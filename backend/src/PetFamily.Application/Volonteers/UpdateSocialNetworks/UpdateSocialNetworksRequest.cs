using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.UpdateSocialNetworks
{
    public record UpdateSocialNetworksRequest(
        Guid VolonteerId,
        IEnumerable<SocialNetworkDTO> SocialNetworks);
}
