using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class ResultModel<IModel> where IModel : class
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Skip { get; set; }
        public IModel[] Data { get; set; } = [];
    }
}
