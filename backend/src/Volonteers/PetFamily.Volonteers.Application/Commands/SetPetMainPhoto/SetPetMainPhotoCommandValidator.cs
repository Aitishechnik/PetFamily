using FluentValidation;

namespace PetFamily.Volonteers.Application.Commands.SetPetMainPhoto
{
    public class SetPetMainPhotoCommandValidator : AbstractValidator<SetPetMainPhotoCommand>
    {
        public SetPetMainPhotoCommandValidator()
        {
            RuleFor(x => x.VolonteerId)
                .NotEmpty();
            RuleFor(x => x.PetId)
                .NotEmpty();
            RuleFor(x => x.MainPhotoPath)
                .NotEmpty();
        }
    }
}
