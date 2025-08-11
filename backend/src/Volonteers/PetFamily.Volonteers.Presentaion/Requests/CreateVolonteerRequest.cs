using PetFamily.Core.Dtos;
using PetFamily.Volonteers.Application.Commands.Create;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record
        CreateVolonteerRequest(
        PersonalDataDto PersonalDataDTO,
        ProfessionalDataDto ProfessionalDataDTO,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<DonationDetailsDto> DonationDetails)
    {
        public CreateVolonteerCommand ToCommand()
        {
            return new CreateVolonteerCommand(
                PersonalDataDTO,
                ProfessionalDataDTO,
                SocialNetworks,
                DonationDetails);
        }
    }
}
