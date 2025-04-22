using System.ComponentModel.DataAnnotations;
using DataAnnotationValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace ProjectSpace.Services.Validation;

internal sealed class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ValidationResult ValidateObject(object value)
    {
        ArgumentNullException.ThrowIfNull(value);

        ValidationContext context = new(value, serviceProvider: _serviceProvider, null);
        List<DataAnnotationValidationResult> results = [];

        bool isValid = Validator.TryValidateObject(value, context, results, validateAllProperties: true);

        if (isValid)
        {
            return ValidationResult.Valid;
        }

        return CreateFailedReult(results);
    }

    private static ValidationResult CreateFailedReult(List<DataAnnotationValidationResult> results)
    {
        Dictionary<string, List<string>> errors = [];

        foreach (var error in results)
        {
            error.ErrorMessage ??= string.Empty;

            foreach (var member in error.MemberNames)
            {
                if (errors.TryGetValue(member, out var memberErrors) is false)
                {
                    memberErrors = [];
                    errors[member] = memberErrors;
                }
                memberErrors.Add(error.ErrorMessage);
            }
        }

        return ValidationResult.FromErrors(errors);
    }
}