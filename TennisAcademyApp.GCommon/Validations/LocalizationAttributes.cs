using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.GCommon.Validations
{
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        private readonly Func<string> _errorMessageFunc;

        public RequiredLocalizedAttribute(string messageKey, Type messageType)
        {
            var property = messageType.GetProperty(messageKey);
            _errorMessageFunc = () => property?.GetValue(null) as string ?? "Required field.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = _errorMessageFunc();
            return base.IsValid(value, validationContext);
        }
    }
    public class RangeLocalizedAttribute : RangeAttribute
    {
        private readonly Func<string> _errorMessageFunc;

        public RangeLocalizedAttribute(double minimum, double maximum, string messageKey, Type messageType)
            : base(minimum, maximum)
        {
            var property = messageType.GetProperty(messageKey);
            _errorMessageFunc = () => property?.GetValue(null) as string ?? "Value out of range.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = _errorMessageFunc();
            return base.IsValid(value, validationContext);
        }
    }
    public class StringLengthLocalizedAttribute : StringLengthAttribute
    {
        private readonly Func<string> _errorMessageFunc;

        public StringLengthLocalizedAttribute(int maximumLength, string messageKey, Type messageType)
            : base(maximumLength)
        {
            var property = messageType.GetProperty(messageKey);
            _errorMessageFunc = () => property?.GetValue(null) as string ?? "Invalid length.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = _errorMessageFunc();
            return base.IsValid(value, validationContext);
        }
    }
}