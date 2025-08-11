using FluentValidation;

namespace PetFamily.Volonteers.Application.Commands.RemovePetPhotos
{
    public class RemovePetPhotosCommandValidator : AbstractValidator<RemovePetPhotosCommand>
    {
        public RemovePetPhotosCommandValidator()
        {
            RuleFor(r => r.FileInfoPath)
                .NotEmpty();

            RuleForEach(r => r.FileInfoPath)
                .NotEmpty()
                .Must(path => !string.IsNullOrWhiteSpace(path.Bucket));
        }
    }
}
