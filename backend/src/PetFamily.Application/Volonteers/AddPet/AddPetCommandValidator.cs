﻿using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.AddPet
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        public AddPetCommandValidator()
        {
            RuleFor(a => a.VolonteerID)
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
                    x.HelthInfo,
                    x.IsNeutered,
                    x.IsVaccinated));

            RuleForEach(a => a.DonationDetails)
                .MustBeValueObject(x => DonationDetails.Create(
                    x.Name,
                    x.Link));

            RuleFor(a => a.PetTypeDTO)
                .MustBeValueObject(x => PetType.Create(
                    x.SpeciesID,
                    x.BreedID));
        }
    }
}
