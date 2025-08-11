using FluentValidation;

namespace PetFamily.Volonteers.Application.Commands.PetDelete
{
    public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
    {
        public DeletePetCommandValidator()
        {
            RuleFor(x => x.VolonteerId).NotEmpty();
            RuleFor(x => x.PetId).NotEmpty();
        }
    }
}
