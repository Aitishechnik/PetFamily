using PetFamily.Core.Dtos;
using PetFamily.Volonteers.Application.Commands.UpdatePetInfo;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record UpdatePetInfoRequest(
        int SerialNumber,
        PetGeneralInfoDto PetGeneralInfoDTO,
        PetCharacteristicsDto PetCharacteristicsDTO,
        PetHealthInfoDto PetHealthInfoDTO,
        PetTypeDto PetTypeDTO,
        IEnumerable<DonationDetailsDto> DonationDetails)
    {
        public UpdatePetInfoCommand ToCommand(Guid volonteerId, Guid petId) => new(
            volonteerId,
            petId,
            SerialNumber,
            PetGeneralInfoDTO,
            PetCharacteristicsDTO,
            PetHealthInfoDTO,
            PetTypeDTO,
            DonationDetails);
    }
}
