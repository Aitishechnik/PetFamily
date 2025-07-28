using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.Commands.AddPet
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        public AddPetCommandValidator()
        {
            RuleFor(a => a.VolonteerId)
                .NotNull()
                .Must(a => a != Guid.Empty);

            RuleFor(a => a.PetGeneralInfoDTO)
                .MustBeValueObject(x => PetGeneralInfo.Create(
                    x.Name, 
                    x.Description, 
                    x.Address, 
                    x.OwnerPhoneNumber, 
                    x.DateOfBirth,
                    x.HelpStatus));

            RuleFor(a => a.PetCharacteristicsDTO)
                .MustBeValueObject(x => PetCharacteristics.Create(
                    x.Color,
                    x.Weight,
                    x.Height));

            RuleFor(a => a.PetHealthInfoDTO)
                .MustBeValueObject(x => PetHealthInfo.Create(
                    x.HealthInfo,
                    x.IsNeutered,
                    x.IsVaccinated));

            RuleForEach(a => a.DonationDetails)
                .MustBeValueObject(x => DonationDetails.Create(
                    x.Name,
                    x.Description));

            RuleFor(a => a.PetTypeDTO)
                .MustBeValueObject(x => PetType.Create(
                    x.SpeciesId,
                    x.BreedId));
        }
    }
}
