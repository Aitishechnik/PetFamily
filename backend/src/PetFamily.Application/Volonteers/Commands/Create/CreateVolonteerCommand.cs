using PetFamily.Application.Abstraction;
using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Commands.Create
{
    public record CreateVolonteerCommand(
        PersonalDataDTO PersonalDataDTO,
        ProfessionalDataDTO ProfessionalDataDTO,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationDetailsDTO> DonationDetails) : ICommand;
}
