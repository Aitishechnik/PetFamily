using PetFamily.Application.Volonteers.Commands.UpdateSocialNetworks;
using PetFamily.Contracts;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdateSocialNetworkRequest(IEnumerable<SocialNetworkDTO> SocialNetworksDTO)
    {
        public UpdateSocialNetworksCommand ToCommand(Guid volonteerId)
        {
            return new UpdateSocialNetworksCommand(
                volonteerId,
                SocialNetworksDTO);
        }
    }
}
