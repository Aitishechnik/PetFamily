using PetFamily.Application.Volonteers.Commands.UpdateMainInfo;
using PetFamily.Contracts;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdateMainInfoRequest(
        PersonalDataDTO PersonalDataDTO, 
        ProfessionalDataDTO ProfessionalDataDTO)
    {
        public UpdateMainInfoCommand ToCommand(Guid volonteerId)
        {
            return new UpdateMainInfoCommand(
                volonteerId,
                new MainInfoDTO(
                    PersonalDataDTO, 
                    ProfessionalDataDTO));
        }
    }
}
