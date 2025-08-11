using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolonteersRepository volonteersRepository,
            [FromKeyedServices(Modules.Volonteers)] IUnitOfWork unitOfWork,
            IValidator<UpdateMainInfoCommand> validator,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateMainInfoCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
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
