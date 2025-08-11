using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.Commands.UpdateDonationDetails
{
    public class UpdateDonationDetailCommandsValidator : AbstractValidator<UpdateDonationDetailsCommand>
    {
        public UpdateDonationDetailCommandsValidator()
        {
            RuleFor(x => x.VolonteerId)
                .NotEmpty();

            RuleForEach(x => x.DonationDetails)
                .MustBeValueObject(u => DonationDetails.Create(u.Name, u.Description));
        }
    }
}
