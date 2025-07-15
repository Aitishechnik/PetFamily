using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
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

        public async Task<Result<Guid, ErrorList>> Handle(
            IValidator<UpdateDonationDetailsCommand> validator,
            UpdateDonationDetailsCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var donationDetails = new List<DonationDetails>();

            foreach (var dd in command.DonationDetails)
                donationDetails.Add(DonationDetails.Create(dd.Name, dd.Link).Value);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                volonteerResult.Value.UpdateDonationDetails(new DonationDetailsWrapper(donationDetails));

                await _unitOfWork.SaveChangesAsync();

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {Id} has been updated with new donation details", volonteerResult.Value.Id);

                return volonteerResult.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while updating donation details for volonteer with id {VolonteerId}", command.VolonteerId);
                return Errors.General
                    .ValueIsInvalid("volonteer donation details update")
                    .ToErrorList();
            }
        }
    }
}
