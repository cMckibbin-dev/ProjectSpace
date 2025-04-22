namespace ProjectSpace.Services.Validation;

internal interface IValidationService
{
    ValidationResult ValidateObject(object value);
}
