using PetFamily.Core.Dtos;
using PetFamily.Volonteers.Application.Commands.UpdateDonationDetails;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record UpdateDonationDetailsRequest(IEnumerable<DonationDetailsDto> DonationDetailsDTO)
    {
        public UpdateDonationDetailsCommand ToCommand(Guid volonteerId)
        {
            return new UpdateDonationDetailsCommand(
                volonteerId,
                DonationDetailsDTO);
        }
    }
}
