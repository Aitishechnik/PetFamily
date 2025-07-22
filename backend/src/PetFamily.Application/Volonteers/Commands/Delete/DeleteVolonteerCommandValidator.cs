using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.Delete
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
