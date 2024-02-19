using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Function executed on initialization of the view model
        /// </summary>
        public virtual void OnInit()
        {

        }

        /// <summary>
        /// Executed when the a dialog opened from this view model was closed and contained parameters.
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedFrom(Dictionary<string, object>? parameters)
        {
        }
    }
}
