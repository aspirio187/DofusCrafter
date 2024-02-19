using DofusCrafter.Data;
using DofusCrafter.Data.Entities;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Models.DofusDb;
using DofusCrafter.UI.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Services
{
    public class ConfectionService : IConfectionService
    {
        private readonly DofusCrafterDbContext _dbContext;
        private readonly DofusDBService _dofusDbService;

        public ConfectionService(DofusCrafterDbContext dbContext, DofusDBService dofusDbService)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (dofusDbService is null)
            {
                throw new ArgumentException(nameof(dofusDbService));
            }

            _dbContext = dbContext;
            _dofusDbService = dofusDbService;
        }

        /// <summary>
        /// Get all the confections from the database
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> where T is <see cref="ConfectionModel"/>
        /// </returns>
        public async Task<IEnumerable<ConfectionModel>> GetConfectionsAsync()
        {
            var confectionsFromDb = _dbContext
                .Confections
                .Include(confection => confection.ConfectionIngredients)
                .ToList();

            if (confectionsFromDb.Count == 0)
            {
                return Enumerable.Empty<ConfectionModel>();
            }

            List<ConfectionModel> confections = [];

            foreach (ConfectionEntity confectionFromDb in confectionsFromDb)
            {
                if (confectionFromDb is null)
                {
                    continue;
                }

                ItemModel? itemFromDofusDb = await _dofusDbService.GetItemAsync(confectionFromDb.ItemId);

                if (itemFromDofusDb is null)
                {
                    continue;
                }

                confections.Add(new ConfectionModel()
                {
                    Id = confectionFromDb.Id,
                    ItemId = confectionFromDb.ItemId,
                    Name = itemFromDofusDb.Name.Fr,
                    Description = itemFromDofusDb.Description.Fr,
                    Image = itemFromDofusDb.Img,
                    Quantity = confectionFromDb.Quantity,
                    TotalPrice = confectionFromDb.ConfectionIngredients.Sum(ci => ci.Price)
                });
            }

            return confections;
        }

        public bool SaveConfection(int itemId, int quantity, List<RegisteredIngredientDto> ingredients)
        {
            if (itemId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(itemId));
            }

            if (ingredients is null)
            {
                throw new ArgumentNullException(nameof(ingredients));
            }

            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(quantity)} {quantity}");
            }

            ConfectionEntity confection = new ConfectionEntity()
            {
                ItemId = itemId,
                Quantity = quantity,
            };

            _dbContext.Confections.Add(confection);

            confection.ConfectionIngredients = ingredients.Select(i => new ConfectionIngredientEntity()
            {
                Price = i.TotalPrice,
                Quantity = i.QuantityRegistered,
                Confection = confection
            }).ToList();

            return _dbContext.SaveChanges() > 0;
        }
    }
}
