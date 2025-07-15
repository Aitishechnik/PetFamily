using PetFamily.Domain.Shared;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PetFamily.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static ErrorList ToErrorList(this ValidationResult validationResult)
        {
            return new ErrorList(
                    validationResult.Errors.Select(
                        e => Error.Validation(
                            e.ErrorCode, e.ErrorMessage, e.PropertyName)));
        }
    }
}
