using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.RemovePetPhotos
{
    public class RemovePetPhotosValidator : AbstractValidator<RemovePetPhotosRequest>
    {
        public RemovePetPhotosValidator()
        {
            RuleFor(r => r.Paths)
                .NotEmpty();

            RuleForEach(r => r.Paths)
                .NotEmpty()
                .Must(path => path.Path.Length <= Constants.MAX_LINK_LENGTH);
        }
    }
}
