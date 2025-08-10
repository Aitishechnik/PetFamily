using PetFamily.Core.Dtos;
using PetFamily.Volonteers.Application.Commands.UpdateMainInfo;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record UpdateMainInfoRequest(
        PersonalDataDto PersonalDataDTO,
        ProfessionalDataDto ProfessionalDataDTO)
    {
        public UpdateMainInfoCommand ToCommand(Guid volonteerId)
        {
            return new UpdateMainInfoCommand(
                volonteerId,
                new MainInfoDto(
                    PersonalDataDTO,
                    ProfessionalDataDTO));
        }
    }
}
