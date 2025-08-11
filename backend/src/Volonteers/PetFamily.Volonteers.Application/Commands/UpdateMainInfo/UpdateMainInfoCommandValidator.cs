using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdateMainInfo
{
    public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoCommandValidator()
        {
            RuleFor(u => u.VolonteerId)
                .NotEmpty()
                .WithErrorCode(Errors.General.ValueIsRequired().Code)
                .WithMessage(Errors.General.ValueIsInvalid().Message);
            RuleFor(u => u.MainInfoDTO.PersonalDataDTO)
                .MustBeValueObject(x => PersonalData.Create(x.FullName, x.Email, x.PhoneNumber));
            RuleFor(u => u.MainInfoDTO.ProfessionalDataDTO)
                .MustBeValueObject(x => ProfessionalData.Create(x.Description, x.ExperienceInYears));
        }
    }
}
