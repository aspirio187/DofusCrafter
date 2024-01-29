using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        private ObservableCollection<ValidationResult> _validationResults = [];
        public ObservableCollection<ValidationResult> ValidationResults
        {
            get => _validationResults;
            set
            {
                _validationResults = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsValid
        {
            get
            {
                bool isValid = Validator.TryValidateObject(this, new ValidationContext(this), ValidationResults, true);
                return isValid == true;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ValidateProperty<T>(ref T origin, T value, [CallerMemberName] string? property = null)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = property };

            if (!Validator.TryValidateProperty(value, context, validationResults))
            {
                var newErrors = validationResults.Where(vr => vr.MemberNames.Contains(property));
                foreach (var validationResult in newErrors)
                {
                    if (!ValidationResults.Any(vr => vr.ErrorMessage == validationResult.ErrorMessage))
                    {
                        ValidationResults.Add(validationResult);
                    }
                }
            }
            else
            {
                foreach (var validationResult in ValidationResults)
                {
                    ValidationResults.Remove(validationResult);
                }
            }

            origin = value;
        }


        protected void CleanResults()
        {
            var uniqueErrors = new HashSet<string>();
            var itemsToRemove = new List<ValidationResult>();

            foreach (var result in _validationResults)
            {
                if (!uniqueErrors.Add(result.ErrorMessage))
                {
                    itemsToRemove.Add(result);
                }
            }

            foreach (var item in itemsToRemove)
            {
                _validationResults.Remove(item);
            }
        }

    }
}
