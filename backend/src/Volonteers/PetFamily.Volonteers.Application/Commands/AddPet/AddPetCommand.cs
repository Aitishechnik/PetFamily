using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolonteerId,
        PetGeneralInfoDto PetGeneralInfoDTO,
        PetCharacteristicsDto PetCharacteristicsDTO,
        PetHealthInfoDto PetHealthInfoDTO,
        IEnumerable<DonationDetailsDto> DonationDetails,
        PetTypeDto PetTypeDTO) : ICommand;
}
