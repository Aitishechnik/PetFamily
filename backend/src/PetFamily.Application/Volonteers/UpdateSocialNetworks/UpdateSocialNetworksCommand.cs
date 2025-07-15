using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolonteerId,
        IEnumerable<SocialNetworkDTO> SocialNetworks);
}
