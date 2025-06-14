using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Contracts;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateMainInfo
{
    public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoRequest>
    {
        public UpdateMainInfoRequestValidator()
        {
            RuleFor(u => u.VolonteerId)
                .NotEmpty().WithErrorCode(Errors.General.ValueIsRequired().Code);
            RuleFor(u => u.Dto.PersonalDataDTO)
                .MustBeValueObject(x => PersonalData.Create(x.FullName, x.Email, x.PhoneNumber));
            RuleFor(u => u.Dto.ProfessionalDataDTO)
                .MustBeValueObject(x => ProfessionalData.Create(x.Description, x.ExperienceInYears));
        }

    }
}
