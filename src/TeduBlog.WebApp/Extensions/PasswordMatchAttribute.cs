using System.ComponentModel.DataAnnotations;

namespace TeduBlog.WebApp.Extensions
{
    public class PasswordMatchAttribute : ValidationAttribute
    {
        private readonly string otherProperty;

        public PasswordMatchAttribute(string otherProperty) : base("{0} and {1} must match.")
        {
            this.otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyValue = validationContext.ObjectType.GetProperty(otherProperty).GetValue(validationContext.ObjectInstance, null);

            if (value == null || value.ToString() == otherPropertyValue?.ToString())
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
