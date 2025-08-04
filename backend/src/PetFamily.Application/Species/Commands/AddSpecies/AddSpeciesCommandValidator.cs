using FluentValidation;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Species.Commands.AddSpecies
{
    public class AddSpeciesCommandValidator : AbstractValidator<AddSpeciesCommand>
    {
        public AddSpeciesCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .MaximumLength(Constants.MAX_NAME_LENGTH);
        }
    }
}
