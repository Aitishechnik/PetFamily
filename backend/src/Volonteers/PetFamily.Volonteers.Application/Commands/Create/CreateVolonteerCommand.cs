using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.Commands.Create
{
    public record CreateVolonteerCommand(
        PersonalDataDto PersonalDataDTO,
        ProfessionalDataDto ProfessionalDataDTO,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<DonationDetailsDto> DonationDetails) : ICommand;
}
