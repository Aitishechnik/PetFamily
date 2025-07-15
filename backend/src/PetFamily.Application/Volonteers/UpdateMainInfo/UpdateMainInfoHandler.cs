using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            IValidator<UpdateMainInfoCommand> validator,
            UpdateMainInfoCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var personalData = PersonalData.Create(
                command.MainInfoDTO.PersonalDataDTO.FullName,
                command.MainInfoDTO.PersonalDataDTO.Email,
                command.MainInfoDTO.PersonalDataDTO.PhoneNumber);

            var professionalData = ProfessionalData.Create(
                command.MainInfoDTO.ProfessionalDataDTO.Description,
                command.MainInfoDTO.ProfessionalDataDTO.ExperienceInYears);

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                volonteerResult.Value.UpdateMainInfo(
                    personalData.Value,
                    professionalData.Value);

                await _unitOfWork.SaveChangesAsync();

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {Id} has been updated", volonteerResult.Value.Id);

                return volonteerResult.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while updating main info for volonteer with id {VolonteerId}", command.VolonteerId);
                return Errors.General
                    .ValueIsInvalid("volonteer main info update")
                    .ToErrorList();
            }
        }
    }
}
