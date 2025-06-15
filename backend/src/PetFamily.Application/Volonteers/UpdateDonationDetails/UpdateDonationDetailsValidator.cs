using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateDonationDetails
{
    public class UpdateDonationDetailsValidator : AbstractValidator<UpdateDonationDetailsRequest>
    {
        public UpdateDonationDetailsValidator()
        {
            RuleFor(x => x.VolonteerId)
                .NotEmpty()
                .WithErrorCode(Errors.General.ValueIsRequired().Code)
                .WithMessage(Errors.General.ValueIsInvalid().Message);
            RuleForEach(x => x.DonationDetails)
                .MustBeValueObject(u => DonationDetails.Create(u.Name, u.Description));
        }
    }
}
