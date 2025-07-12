using FluentValidation;

namespace PetFamily.Application.Volonteers.ShiftPetPosition
{
    public class ShiftPetPositionValidator : AbstractValidator<ShiftPetPositionRequest>
    {
        public ShiftPetPositionValidator()
        {
            RuleFor(c => c.VoloteerId)
                .NotEmpty();

            RuleFor(c => c.PetId)
                .NotEmpty();

            RuleFor(c => c.NewPosition)
                .Must(value => value > 0);
        }
    }
}
