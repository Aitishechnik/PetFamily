using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.Commands.Create
{
    public class CreateVolonteerCommandValidator : AbstractValidator<CreateVolonteerCommand>
    {
        public CreateVolonteerCommandValidator()
        {
            RuleFor(c => c.PersonalDataDTO)
                .MustBeValueObject(x => PersonalData.Create(x.FullName, x.Email, x.PhoneNumber));

            RuleFor(c => c.ProfessionalDataDTO)
                .MustBeValueObject(x => ProfessionalData.Create(x.Description, x.ExperienceInYears));

            RuleForEach(c => c.SocialNetworks)
                .MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Link));

            RuleForEach(c => c.DonationDetails)
                .MustBeValueObject(x => DonationDetails.Create(x.Name, x.Description));
        }
    }
}
