using DofusCrafter.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.Data
{
    public class DofusCrafterDbContext : DbContext
    {
        public string DbPath { get; }

        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<ConfectionEntity> Confections { get; set; }
        public DbSet<ConfectionIngredientEntity> ConfectionsIngredients { get; set; }

        public DofusCrafterDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            DbPath = Path.Join(path, "dofus-crafter.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data source={DbPath}");
        }
    }
}
