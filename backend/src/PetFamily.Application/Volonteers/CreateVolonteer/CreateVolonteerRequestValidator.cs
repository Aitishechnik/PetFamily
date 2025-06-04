using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public class CreateVolonteerRequestValidator : AbstractValidator<CreateVolonteerRequest>
    {
        public CreateVolonteerRequestValidator()
        {
            RuleFor(c => new { c.fullName, c.email, c.phoneNumber }).MustBeValueObject(x => PersonalData.Create(x.fullName, x.email,x.phoneNumber));

            RuleFor(c => new { c.description, c.experienceInYears}).MustBeValueObject(x => ProfessionalData.Create(x.description, x.experienceInYears));

            RuleFor(c => new { c.socialNetworkName, c.socialNetworkLink}).MustBeValueObject(x => SocialNetwork.Create(x.socialNetworkName, x.socialNetworkLink));

            RuleFor(c => new { c.donationDetailsName, c.donationDetailsDescription }).MustBeValueObject(x => DonationDetails.Create(x.donationDetailsName,x.donationDetailsDescription));
        }
    }
}
