﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.Forms
{
    public class ConfectionForm
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Slug { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<ConfectionIngredientForm> ConfectionIngredients { get; set; } = [];
    }
}
