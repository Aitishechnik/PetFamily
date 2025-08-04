using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.AddBreed
{
    public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
    {
        public AddBreedCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .MaximumLength(Constants.MAX_NAME_LENGTH);
        }
    }
}
