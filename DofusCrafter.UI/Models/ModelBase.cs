using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models
{
    /// <summary>
    /// Base class for models that provides property change notification and validation functionality.
    /// </summary>
    public class ModelBase : INotifyPropertyChanged
    {
        protected static readonly ResourceManager ResourceManager =
            Globalization.Globalization.Initialize(typeof(Globalization.Globalization).Namespace + ".strings");

        private ObservableCollection<ValidationResult> _validationResults = new ObservableCollection<ValidationResult>();

        /// <summary>
        /// Collection of validation results for the model.
        /// </summary>
        public ObservableCollection<ValidationResult> ValidationResults
        {
            get => _validationResults;
            set
            {
                _validationResults = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the model is valid based on its validation results.
        /// </summary>
        public bool IsValid
        {
            get
            {
                bool isValid = Validator.TryValidateObject(this, new ValidationContext(this), ValidationResults, true);
                return isValid == true;
            }
        }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for a specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Validates a property value and updates the validation results collection.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="origin">Reference to the original property value.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="property">The name of the property being validated.</param>
        protected void ValidateProperty<T>(ref T origin, T value, [CallerMemberName] string property = "")
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this)
            {
                MemberName = property
            };

            if (!Validator.TryValidateProperty(value, context, validationResults))
            {
                IEnumerable<ValidationResult> newErrors = validationResults.Where(vr => vr.MemberNames.Contains(property));

                foreach (ValidationResult validationResult in newErrors)
                {
                    if (validationResult is null || string.IsNullOrEmpty(validationResult.ErrorMessage))
                    {
                        continue;
                    }

                    bool validationResultExist = ValidationResults
                        .Any(vr => string.IsNullOrWhiteSpace(vr.ErrorMessage) ||
                                    vr.ErrorMessage.Equals(validationResult.ErrorMessage));

                    if (!validationResultExist)
                    {
                        ValidationResults.Add(validationResult);
                    }
                }
            }
            else
            {
                for (int i = 0; i < ValidationResults.Count; i++)
                {
                    ValidationResult validationResult = ValidationResults[i];

                    if (validationResult.MemberNames.Equals(property))
                    {
                        ValidationResults.Remove(validationResult);
                        i--;
                    }
                }
            }

            origin = value;
            NotifyPropertyChanged(property);
        }

        /// <summary>
        /// Removes duplicate validation results from the validation results collection.
        /// </summary>
        protected void CleanResults()
        {
            HashSet<string> uniqueErrors = new HashSet<string>();
            List<ValidationResult> itemsToRemove = new List<ValidationResult>();

            foreach (ValidationResult result in _validationResults)
            {
                if (result is null || string.IsNullOrEmpty(result.ErrorMessage))
                {
                    continue;
                }

                if (!uniqueErrors.Add(result.ErrorMessage))
                {
                    itemsToRemove.Add(result);
                }
            }

            foreach (ValidationResult item in itemsToRemove)
            {
                _validationResults.Remove(item);
            }
        }
    }
}
