using FluentValidation;

namespace PetFamily.Application.Volonteers.RemovePetPhotos
{
    public class RemovePetPhotosValidator : AbstractValidator<RemovePetPhotosRequest>
    {
        public RemovePetPhotosValidator()
        {
            RuleFor(r => r.FileInfo)
                .NotEmpty();

            RuleForEach(r => r.FileInfo)
                .NotEmpty()
                .Must(path => !string.IsNullOrWhiteSpace(path.Bucket));
        }
    }
}
