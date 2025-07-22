using FluentValidation;

namespace PetFamily.Application.Volonteers.Commands.RemovePetPhotos
{
    public class RemovePetPhotosCommandValidator : AbstractValidator<RemovePetPhotosCommand>
    {
        public RemovePetPhotosCommandValidator()
        {
            RuleFor(r => r.FileInfo)
                .NotEmpty();

            RuleForEach(r => r.FileInfo)
                .NotEmpty()
                .Must(path => !string.IsNullOrWhiteSpace(path.Bucket));
        }
    }
}
