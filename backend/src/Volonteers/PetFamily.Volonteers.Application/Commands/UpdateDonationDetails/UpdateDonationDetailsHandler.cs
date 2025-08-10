using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdateDonationDetails
{
    public class UpdateDonationDetailsHandler : ICommandHandler<Guid, UpdateDonationDetailsCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateDonationDetailsCommand> _validator;
        private readonly ILogger<UpdateDonationDetailsHandler> _logger;

        public UpdateDonationDetailsHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateDonationDetailsCommand> validator,
            ILogger<UpdateDonationDetailsHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateDonationDetailsCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var donationDetails = new List<DonationDetails>();

            foreach (var dd in command.DonationDetails)
                donationDetails.Add(DonationDetails.Create(dd.Name, dd.Description).Value);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                volonteerResult.Value.UpdateDonationDetails(donationDetails);

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
