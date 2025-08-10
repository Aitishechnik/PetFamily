using PetFamily.Core.Dtos;
using PetFamily.Volonteers.Application.Commands.AddPet;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record AddPetRequest(
        PetGeneralInfoDto PetGeneralInfoDTO,
        PetCharacteristicsDto PetCharacteristicsDTO,
        PetHealthInfoDto PetHealthInfoDTO,
        IEnumerable<DonationDetailsDto> DonationDetails,
        PetTypeDto PetTypeDTO
        )
    {
        public AddPetCommand ToCommand(Guid volonteerId)
        {
            return new AddPetCommand(
                volonteerId,
                PetGeneralInfoDTO,
                PetCharacteristicsDTO,
                PetHealthInfoDTO,
                DonationDetails,
                PetTypeDTO);
        }
    }
}
