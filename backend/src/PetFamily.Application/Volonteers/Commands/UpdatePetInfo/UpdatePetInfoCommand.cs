using PetFamily.Application.Abstraction;
using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Commands.UpdatePetInfo
{
    public record UpdatePetInfoCommand(
        Guid VolonteerId,
        Guid PetId,
        int SerialNumber,
        PetGeneralInfoDTO PetGeneralInfoDTO,
        PetCharacteristicsDTO PetCharacteristicsDTO,
        PetHealthInfoDTO PetHealthInfoDTO,
        PetTypeDTO PetTypeDTO,
        IEnumerable<DonationDetailsDTO> DonationDetails) : ICommand;
}
