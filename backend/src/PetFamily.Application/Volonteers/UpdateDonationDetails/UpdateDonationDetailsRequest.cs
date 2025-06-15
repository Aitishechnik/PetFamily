using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.UpdateDonationDetails
{
    public record UpdateDonationDetailsRequest(
        Guid VolonteerId,
        IEnumerable<DonationDetailsDTO> DonationDetails);
}
