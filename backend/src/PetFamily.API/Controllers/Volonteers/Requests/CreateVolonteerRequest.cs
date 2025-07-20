using PetFamily.Application.Volonteers.Commands.Create;
using PetFamily.Contracts;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record 
        CreateVolonteerRequest(
        PersonalDataDTO PersonalDataDTO,
        ProfessionalDataDTO ProfessionalDataDTO,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationDetailsDTO> DonationDetails)
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
