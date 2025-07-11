﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateDonationDetails
{
    public class UpdateDonationDetailsHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateDonationDetailsHandler> _logger;

        public UpdateDonationDetailsHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateDonationDetailsHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateDonationDetailsRequest updateDonationDetailsRequest,
            CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransaction();

            try
            {
                var volonteerResult = await _volonteersRepository.GetById(updateDonationDetailsRequest.VolonteerId, cancellationToken);
                if (volonteerResult.IsFailure)
                    return volonteerResult.Error;

                var donationDetails = new List<DonationDetails>();

                foreach (var dd in updateDonationDetailsRequest.DonationDetails)
                    donationDetails.Add(DonationDetails.Create(dd.Name, dd.Description).Value);

                volonteerResult.Value.UpdateDonationDetails(new DonationDetailsWrapper(donationDetails));

                await _unitOfWork.SaveChanges();

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {Id} has been updated with new donation details", volonteerResult.Value.Id);

                return volonteerResult.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while updating donation details for volonteer with id {VolonteerId}", updateDonationDetailsRequest.VolonteerId);
                return Errors.General.ValueIsInvalid("volonteer donation details update");
            }
        }
    }
}
