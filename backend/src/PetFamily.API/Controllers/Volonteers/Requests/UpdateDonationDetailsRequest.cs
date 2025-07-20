using PetFamily.Application.Volonteers.Commands.UpdateDonationDetails;
using PetFamily.Contracts;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdateDonationDetailsRequest(IEnumerable<DonationDetailsDTO> DonationDetailsDTO)
    {
        public UpdateDonationDetailsCommand ToCommand(Guid volonteerId)
        {
            return new UpdateDonationDetailsCommand(
                volonteerId,
                DonationDetailsDTO);
        }
    }
}
