using PetFamily.Application.Abstraction;
using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Commands.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolonteerId,
        IEnumerable<SocialNetworkDTO> SocialNetworks) : ICommand;
}
