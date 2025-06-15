using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateDonationDetails
{
    public class UpdateDonationDetailsHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<UpdateDonationDetailsHandler> _logger;

        public UpdateDonationDetailsHandler(
            IVolonteersRepository volonteersRepository,
            ILogger<UpdateDonationDetailsHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateDonationDetailsRequest updateDonationDetailsRequest,
            CancellationToken cancellationToken)
        {
            var volonteerResult = await _volonteersRepository.GetById(updateDonationDetailsRequest.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error;

            var donationDetails = new List<DonationDetails>();

            foreach (var dd in updateDonationDetailsRequest.DonationDetails)
                donationDetails.Add(DonationDetails.Create(dd.Name, dd.Description).Value);

            volonteerResult.Value.UpdateDonationDetails(new DonationDetailsWrapper(donationDetails));

            var result = await _volonteersRepository.Save(volonteerResult.Value, cancellationToken);

            _logger.LogInformation("Volonteer with id {result} has been updated with new donation details", result);

            return result;
        }
    }
}
