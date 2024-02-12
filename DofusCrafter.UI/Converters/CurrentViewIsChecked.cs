using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DofusCrafter.UI.Converters
{
    public class CurrentViewIsChecked : IValueConverter
    {
        /// <summary>
        /// Return true or false if the current active content control (<paramref name="value"/>) name
        /// is equals to <paramref name="parameter"/>
        /// </summary>
        /// <param name="value">The currently active content control</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The name of the content control assigned to the RadioButton</param>
        /// <param name="culture"></param>
        /// <returns>
        /// true If the currently active content control is equals to the given name in parameter.
        /// false Otherwise
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ContentControl contentControl || value is null)
            {
                return false;
            }

            if (parameter is null)
            {
                return false;
            }

            if (!contentControl.DependencyObjectType.Name.Equals((string)parameter))
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Convert <paramref name="value"/> to the opposite of its value
        /// </summary>
        /// <param name="value">The current boolean value of the RadioButton</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The name of the content control assigned to the RadioButton</param>
        /// <param name="culture"></param>
        /// <returns>
        /// true If value is false. false If the value is true
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
