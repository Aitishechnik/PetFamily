using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdatePetInfo
{
    public class UpdatePetInfoCommandValidator : AbstractValidator<UpdatePetInfoCommand>
    {
        public UpdatePetInfoCommandValidator()
        {
            RuleFor(x => x.VolonteerId)
                .NotEmpty();

            RuleFor(x => x.PetId)
                .NotEmpty();

            RuleFor(x => x.SerialNumber)
                .GreaterThan(0);

            RuleFor(x => x.PetGeneralInfoDTO.Name)
                .NotEmpty();

            RuleFor(x => x.PetGeneralInfoDTO.Description)
                .NotEmpty();

            RuleFor(x => x.PetGeneralInfoDTO.Address)
                .NotEmpty();

            RuleFor(x => x.PetGeneralInfoDTO.OwnerPhoneNumber)
                .NotEmpty();

            RuleFor(x => x.PetGeneralInfoDTO.DateOfBirth)
                .LessThan(DateTime.Now);

            RuleFor(x => x.PetGeneralInfoDTO.HelpStatus)
                .IsInEnum();

            RuleFor(x => x.PetCharacteristicsDTO.Color)
                .NotEmpty();

            RuleFor(x => x.PetCharacteristicsDTO.Weight)
                .GreaterThan(0);

            RuleFor(x => x.PetCharacteristicsDTO.Height)
                .GreaterThan(0);
            RuleFor(x => x.PetHealthInfoDTO.HealthInfo)
                .NotEmpty();

            RuleFor(x => x.PetHealthInfoDTO.IsNeutered)
                .NotNull();

            RuleFor(x => x.PetHealthInfoDTO.IsVaccinated)
                .NotNull();

            RuleFor(x => x.PetTypeDTO.SpeciesId)
                .NotEmpty();

            RuleFor(x => x.PetTypeDTO.BreedId)
                .NotEmpty();

            RuleForEach(x => x.DonationDetails)
                .MustBeValueObject(
                dto => DonationDetails.Create(
                    dto.Name,
                    dto.Description));
        }
    }
}
