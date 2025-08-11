using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.AddPetPhotos
{
    public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
    {
        public AddPetPhotosCommandValidator()
        {
            RuleFor(a => a.Content)
                .NotNull()
                .NotEmpty();

            RuleForEach(a => a.Content)
                .NotNull()
                .Must(a => a.Length <= Constants.MAX_FILE_SIZE);

            RuleFor(a => a.VolonteerId).
                Must(x => x != Guid.Empty);

            RuleFor(a => a.PetId).
                Must(x => x != Guid.Empty);

        }
    }
}
