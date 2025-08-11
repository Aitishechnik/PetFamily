using FluentValidation;
using PetFamily.SharedKernal;

namespace PetFamily.Volonteers.Application.Commands.Delete
{
    public class DeleteVolonteerCommandValidator : AbstractValidator<DeleteVolonteerCommand>
    {
        public DeleteVolonteerCommandValidator()
        {
            RuleFor(u => u.VolonteerId)
                .NotEmpty()
                .WithErrorCode(Errors.General.ValueIsRequired().Code)
                .WithMessage(Errors.General.ValueIsInvalid().Message);
        }
    }
}
