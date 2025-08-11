using PetFamily.Core.Dtos;
using PetFamily.Volonteers.Application.Commands.UpdateSocialNetworks;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record UpdateSocialNetworkRequest(IEnumerable<SocialNetworkDto> SocialNetworksDTO)
    {
        public UpdateSocialNetworksCommand ToCommand(Guid volonteerId)
        {
            return new UpdateSocialNetworksCommand(
                volonteerId,
                SocialNetworksDTO);
        }
    }
}
