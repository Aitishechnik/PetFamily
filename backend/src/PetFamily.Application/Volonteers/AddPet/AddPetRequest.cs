using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.AddPet
{
    public record AddPetRequest(
        Guid VolonteerID,
        PetGeneralInfoDTO PetGeneralInfoDTO,
        PetCharacteristicsDTO PetCharacteristicsDTO,
        PetHealthInfoDTO PetHealthInfoDTO,
        IEnumerable<DonationDetailsDTO> DonationDetails,
        PetTypeDTO PetTypeDTO);
}
