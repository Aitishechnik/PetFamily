using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.UpdateMainInfo
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
