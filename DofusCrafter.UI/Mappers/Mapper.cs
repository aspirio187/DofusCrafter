using DofusCrafter.Data.Entities;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Models.DofusDb;
using DofusCrafter.UI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Mappers
{
    public static class Mapper
    {
        public static SaleEntity MapToSaleEntity(this SoldItemModel soldItemModel)
        {
            return new SaleEntity
            {
                ItemName = soldItemModel.Name,
                Quantity = int.Parse(soldItemModel.Quantity),
                UnitaryPrice = decimal.Parse(soldItemModel.Price),
                SaleDate = soldItemModel.SoldDate
            };
        }

        public static RecipeDto MapToRecipeDto(this RecipeModel model)
        {
            RecipeDto recipe = new RecipeDto()
            {
                Id = model.Result.Id,
                ItemName = model.ResultName.Fr,
                Description = model.Result.Description.Fr,
                Level = model.Result.Level,
                Img = model.Result.Img,
            };

            for (int i = 0; i < model.IngredientIds.Length; i++)
            {
                int ingredientId = model.IngredientIds[i];

                ItemModel? ingredient = model.Ingredients.FirstOrDefault(x => x.Id == ingredientId);

                if (ingredient is null)
                {
                    continue;
                }

                recipe.Ingredients.Add(new IngredientDto()
                {
                    Id = ingredientId,
                    Name = ingredient.Name.Fr,
                    Description = ingredient.Description.Fr,
                    Level = ingredient.Level,
                    Quantity = model.Quantities[i],
                    Img = ingredient.Img
                });
            }

            return recipe;
        }
    }
}
