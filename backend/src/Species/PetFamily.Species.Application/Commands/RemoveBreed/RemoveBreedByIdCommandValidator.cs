using FluentValidation;

namespace PetFamily.Species.Application.Commands.RemoveBreed
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
