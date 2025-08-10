using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.Commands.UpdateDonationDetails
{
    public record UpdateDonationDetailsCommand(
        Guid VolonteerId,
        IEnumerable<DonationDetailsDto> DonationDetails) : ICommand;
}
