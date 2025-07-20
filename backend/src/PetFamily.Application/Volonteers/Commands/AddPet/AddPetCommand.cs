using PetFamily.Application.Abstraction;
using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolonteerID,
        PetGeneralInfoDTO PetGeneralInfoDTO,
        PetCharacteristicsDTO PetCharacteristicsDTO,
        PetHealthInfoDTO PetHealthInfoDTO,
        IEnumerable<DonationDetailsDTO> DonationDetails,
        PetTypeDTO PetTypeDTO) : ICommand;
}
