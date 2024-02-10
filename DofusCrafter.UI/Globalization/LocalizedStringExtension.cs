using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace DofusCrafter.UI.Globalization
{
    public class LocalizedStringExtension : MarkupExtension
    {
        private readonly string _resourceKey;

        public LocalizedStringExtension(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            return Globalization.GetString(_resourceKey);
        }
    }
}
