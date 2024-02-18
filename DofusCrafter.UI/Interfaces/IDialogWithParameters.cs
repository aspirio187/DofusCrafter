using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Interfaces
{
    public interface IDialogWithParameters
    {
        void OnNavigatedTo(Dictionary<string, object> parameters);
    }
}
