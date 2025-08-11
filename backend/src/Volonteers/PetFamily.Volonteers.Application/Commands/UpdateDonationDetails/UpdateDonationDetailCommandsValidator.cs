using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdateDonationDetails
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
