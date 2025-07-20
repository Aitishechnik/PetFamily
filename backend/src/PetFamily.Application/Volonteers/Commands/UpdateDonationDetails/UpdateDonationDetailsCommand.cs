using PetFamily.Application.Abstraction;
using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Commands.UpdateDonationDetails
{
    public record UpdateDonationDetailsCommand(
        Guid VolonteerId,
        IEnumerable<DonationDetailsDTO> DonationDetails) : ICommand;
}
