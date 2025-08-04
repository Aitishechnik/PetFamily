using FluentValidation;

namespace PetFamily.Application.Volonteers.Commands.ShiftPetPosition
{
    public class ShiftPetPositionCommandValidator : AbstractValidator<ShiftPetPositionCommand>
    {
        public ShiftPetPositionCommandValidator()
        {
            RuleFor(c => c.VolonteerId)
                .NotEmpty();

            RuleFor(c => c.PetId)
                .NotEmpty();

            RuleFor(c => c.NewPosition)
                .Must(value => value > 0);
        }
    }
}
