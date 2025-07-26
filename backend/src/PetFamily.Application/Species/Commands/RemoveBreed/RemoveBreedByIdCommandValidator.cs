using FluentValidation;

namespace PetFamily.Application.Species.Commands.RemoveBreed
{
    public class RemoveBreedByIdCommandValidator : AbstractValidator<RemoveBreedByIdCommand>
    {
        public RemoveBreedByIdCommandValidator()
        {
            RuleFor(command => command.BreedId)
                .NotEmpty();
        }
    }
}
