using PetFamily.Application.Volonteers.Commands.UpdatePetInfo;
using PetFamily.Contracts;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdatePetInfoRequest(
        int SerialNumber,
        PetGeneralInfoDTO PetGeneralInfoDTO,
        PetCharacteristicsDTO PetCharacteristicsDTO,
        PetHealthInfoDTO PetHealthInfoDTO,
        PetTypeDTO PetTypeDTO,
        IEnumerable<DonationDetailsDTO> DonationDetails)
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
