using FluentValidation;

namespace PetFamily.Application.Volonteers.Commands.ChangePetStatus
{
    public class ChangePetStatusCommandValidator : AbstractValidator<ChangePetStatusCommand>
    {
        public ChangePetStatusCommandValidator()
        {
            RuleFor(x => x.VolonteerId)
                .NotEmpty();
            RuleFor(x => x.PetId)
                .NotEmpty();
            RuleFor(x => x.NewPetStatus)
                .IsInEnum();
        }
    }
}
