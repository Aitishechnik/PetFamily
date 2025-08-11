using FluentValidation;
using PetFamily.SharedKernal;

namespace PetFamily.Species.Application.Commands.AddBreed
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
