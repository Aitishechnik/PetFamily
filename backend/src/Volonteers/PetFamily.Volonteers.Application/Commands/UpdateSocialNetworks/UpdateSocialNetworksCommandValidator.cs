using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
    {
        public UpdateSocialNetworksCommandValidator()
        {
            RuleFor(u => u.VolonteerId)
                .NotEmpty()
                .WithErrorCode(Errors.General.ValueIsRequired().Code)
                .WithMessage(Errors.General.ValueIsInvalid().Message);
            RuleForEach(u => u.SocialNetworks)
                .MustBeValueObject(u => SocialNetwork.Create(u.Name, u.Link));
        }
    }
}
