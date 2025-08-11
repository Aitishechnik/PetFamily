using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.Commands.UpdatePetInfo
{
    public record UpdatePetInfoCommand(
        Guid VolonteerId,
        Guid PetId,
        int SerialNumber,
        PetGeneralInfoDto PetGeneralInfoDTO,
        PetCharacteristicsDto PetCharacteristicsDTO,
        PetHealthInfoDto PetHealthInfoDTO,
        PetTypeDto PetTypeDTO,
        IEnumerable<DonationDetailsDto> DonationDetails) : ICommand;
}
