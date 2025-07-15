using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.UpdateDonationDetails
{
    public record UpdateDonationDetailsCommand(
        Guid VolonteerId,
        IEnumerable<DonationDetailsDTO> DonationDetails);
}
