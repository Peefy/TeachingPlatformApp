using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Prism.Mvvm;

using DuGu.NetFramework.Logs;



namespace TeachingPlatformApp.Validations
{
    public class ValidatableObject<T> : BindableBase , IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private readonly ObservableCollection<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations => _validations;

        public ObservableCollection<string> Errors => _errors;

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                SetProperty(ref _value, value);
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }

            set
            {
                SetProperty(ref _isValid, value);
            }
        }

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new ObservableCollection<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            try
            {
                Errors.Clear();

                IEnumerable<string> errors = _validations.Where(v => !v.Check(Value))
                                                         .Select(v => v.ValidationMessage);

                foreach (var error in errors)
                {
                    Errors.Add(error);
                }

                IsValid = !Errors.Any();

                return this.IsValid;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentLogger().Error(ex);
                return false;
            }

        }
    }
}
