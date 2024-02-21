using DofusCrafter.Data;
using DofusCrafter.Data.Entities;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Models.DofusDb;
using DofusCrafter.UI.Models.Dtos;
using DofusCrafter.UI.Models.Forms;
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
            List<ConfectionEntity> confectionsFromDb = [.. _dbContext.Confections.Include(confection => confection.ConfectionIngredients)];

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
                    Slug = itemFromDofusDb.Slug.Fr,
                    Description = itemFromDofusDb.Description.Fr,
                    Image = itemFromDofusDb.Img,
                    Quantity = confectionFromDb.Quantity,
                    TotalPrice = confectionFromDb.ConfectionIngredients.Sum(ci => ci.Price),
                    CreatedAt = confectionFromDb.CreatedAt,
                });
            }

            return confections;
        }

        /// <summary>
        /// Get all the confections and filter them
        /// </summary>
        /// <param name="searchQuery">The array of filters to apply on the list</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<ConfectionModel>> GetConfectionsAsync(string[] searchQuery)
        {
            if (searchQuery is null)
            {
                throw new ArgumentNullException(nameof(searchQuery));
            }

            var confections = await GetConfectionsAsync();

            List<ConfectionModel> finalResult = [];

            foreach (string word in searchQuery)
            {
                IEnumerable<ConfectionModel> result = confections.Where(c => c.Slug.Contains(word) || c.Name.Contains(word));

                finalResult.AddRange(result);
            }

            return finalResult;
        }

        /// <summary>
        /// Get a confection from the databse by its id
        /// </summary>
        /// <param name="id">The id of the confection</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ConfectionModel?> GetConfectionAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            ConfectionEntity? confectionFromDb =
                _dbContext
                    .Confections
                    .Include(c => c.ConfectionIngredients)
                    .SingleOrDefault(c => c.Id == id);

            if (confectionFromDb is null)
            {
                return null;
            }

            ItemModel? itemFromDofusDb = await _dofusDbService.GetItemAsync(confectionFromDb.ItemId);

            if (itemFromDofusDb is null)
            {
                return null;
            }

            return new ConfectionModel()
            {
                Id = confectionFromDb.Id,
                ItemId = confectionFromDb.ItemId,
                Name = itemFromDofusDb.Name.Fr,
                Slug = itemFromDofusDb.Slug.Fr,
                Description = itemFromDofusDb.Description.Fr,
                Image = itemFromDofusDb.Img,
                Quantity = confectionFromDb.Quantity,
                TotalPrice = confectionFromDb.ConfectionIngredients.Sum(ci => ci.Price),
                CreatedAt = confectionFromDb.CreatedAt,
            };
        }

        /// <summary>
        /// Create a new confection with its ingredients and save it in the local database
        /// </summary>
        /// <param name="confection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool SaveConfection(ConfectionForm confection)
        {
            if (confection is null)
            {
                throw new ArgumentNullException(nameof(confection));
            }

            ConfectionEntity confectionEntity = new ConfectionEntity()
            {
                ItemId = confection.ItemId,
                Quantity = confection.Quantity,
                Slug = confection.Slug,
            };

            _dbContext.Confections.Add(confectionEntity);

            confectionEntity.ConfectionIngredients = confection
                .ConfectionIngredients
                .Select(i =>
                    new ConfectionIngredientEntity()
                    {
                        Price = i.Price,
                        Quantity = i.Quantity,
                        Confection = confectionEntity
                    })
                .ToList();

            return _dbContext.SaveChanges() > 0;
        }
    }
}
