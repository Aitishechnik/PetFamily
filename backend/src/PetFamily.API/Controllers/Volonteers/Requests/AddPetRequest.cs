using PetFamily.Application.Volonteers.AddPet;
using PetFamily.Contracts;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record AddPetRequest(
        PetGeneralInfoDTO PetGeneralInfoDTO,
        PetCharacteristicsDTO PetCharacteristicsDTO,
        PetHealthInfoDTO PetHealthInfoDTO,
        IEnumerable<DonationDetailsDTO> DonationDetails,
        PetTypeDTO PetTypeDTO
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
