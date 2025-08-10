using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.Commands.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolonteerId,
        IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
}
