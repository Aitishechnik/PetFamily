using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdatePetInfo
{
    public class UpdatePetInfoHandler : ICommandHandler<UpdatePetInfoCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolonteersRepository _volonteerRepository;
        private readonly ISpeciesContract _speciesContract;
        private readonly IValidator<UpdatePetInfoCommand> _validator;
        private readonly ILogger<UpdatePetInfoHandler> _logger;

        public UpdatePetInfoHandler(
            [FromKeyedServices(Modules.Volonteers)] IUnitOfWork unitOfWork,
            IVolonteersRepository volonteerRepository,
            ISpeciesContract speciesContract,
            IValidator<UpdatePetInfoCommand> validator,
            ILogger<UpdatePetInfoHandler> logger
            )
        {
            _unitOfWork = unitOfWork;
            _volonteerRepository = volonteerRepository;
            _speciesContract = speciesContract;
            _validator = validator;
            _logger = logger;
        }
        public async Task<UnitResult<ErrorList>> Handle(
            UpdatePetInfoCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command,
                cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorList();

            var isSpeciesAndBreedExist = await _speciesContract
                .IsSpeciesAndBreedExist(new IsSpeciesAndBreedExistRequest(
                    command.PetTypeDTO.SpeciesId,
                    command.PetTypeDTO.BreedId),
                cancellationToken);

            if (isSpeciesAndBreedExist == false)
            {
                _logger.LogError("Specied or Breed was not found");
                return Errors.General.ValueIsInvalid(
                    "Species or Breed was not found").ToErrorList();
            }

            var volonteerResult = await _volonteerRepository.GetById(
                command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var petResult = volonteerResult.Value.GetPetById(command.PetId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var volonteer = volonteerResult.Value;
            var pet = petResult.Value;

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var updateResult = volonteer.UpdatePetInfo(
                    command.PetId,
                    command.SerialNumber,
                    command.PetGeneralInfoDTO.Name,
                    command.PetGeneralInfoDTO.Description,
                    command.PetGeneralInfoDTO.Address,
                    command.PetGeneralInfoDTO.OwnerPhoneNumber,
                    command.PetGeneralInfoDTO.DateOfBirth,
                    command.PetGeneralInfoDTO.HelpStatus,
                    command.PetCharacteristicsDTO.Color,
                    command.PetCharacteristicsDTO.Weight,
                    command.PetCharacteristicsDTO.Height,
                    command.PetHealthInfoDTO.HealthInfo,
                    command.PetHealthInfoDTO.IsNeutered,
                    command.PetHealthInfoDTO.IsVaccinated,
                    command.PetTypeDTO.SpeciesId,
                    command.PetTypeDTO.BreedId,
                    command.DonationDetails.Select(
                        x => DonationDetails
                        .Create(x.Name, x.Description).Value));
                if (updateResult.IsFailure)
                {
                    _logger.LogError(
                        "Failed to update pet info for volonteer with id {VolonteerId}: {ErrorMessage}",
                        command.VolonteerId, updateResult.Error.Message);
                    return updateResult.Error.ToErrorList();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation(
                    "Volonteer with id {Id} has been updated with new pet info",
                    volonteer.Id);

                return UnitResult.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(
                    ex,
                    "Error occurred while updating pet info for volonteer with id {VolonteerId}", command.VolonteerId);
                return Error.Failure(
                    "internal.error", "Faild to update pet")
                    .ToErrorList();
            }
        }
    }
}
