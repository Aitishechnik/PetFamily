using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Volonteers.Domain.Entities;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.AddPet
{
    public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ISpeciesContract _speciesContract;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetHandler> _logger;

        public AddPetHandler(
            IVolonteersRepository volonteersRepository,
            ISpeciesContract speciesContract,
            IUnitOfWork unitOfWork,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _speciesContract = speciesContract;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var isSpeciesAndBreedExist = await _speciesContract
                .IsSpeciesAndBreedExist(
                new IsSpeciesAndBreedExistRequest(
                    command.PetTypeDTO.SpeciesId, 
                    command.PetTypeDTO.BreedId),
                cancellationToken);

            if (isSpeciesAndBreedExist == false)
            {
                _logger.LogError("Specied or Breed was not found");
                return Errors.General.ValueIsInvalid(
                    "Species or Breed was not found").ToErrorList();
            }

            var volonteerResult = await _volonteersRepository.GetById(
                command.VolonteerId, cancellationToken);

            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var volonteer = volonteerResult.Value;

            var donationDetailsCollection = new List<DonationDetails>();
            foreach (var donationDetail in command.DonationDetails)
                donationDetailsCollection.Add(DonationDetails.Create(
                    donationDetail.Name,
                    donationDetail.Description).
                    Value);

            var pet = new Pet(
                PetGeneralInfo.Create(
                    command.PetGeneralInfoDTO.Name,
                    command.PetGeneralInfoDTO.Description,
                    command.PetGeneralInfoDTO.Address,
                    command.PetGeneralInfoDTO.OwnerPhoneNumber,
                    command.PetGeneralInfoDTO.DateOfBirth,
                    command.PetGeneralInfoDTO.HelpStatus).Value,
                PetCharacteristics.Create(
                    command.PetCharacteristicsDTO.Color,
                    command.PetCharacteristicsDTO.Weight,
                    command.PetCharacteristicsDTO.Height).Value,
                PetHealthInfo.Create(
                    command.PetHealthInfoDTO.HealthInfo,
                    command.PetHealthInfoDTO.IsNeutered,
                    command.PetHealthInfoDTO.IsVaccinated).Value,
                donationDetailsCollection,
                PetType.Create(
                    command.PetTypeDTO.SpeciesId,
                    command.PetTypeDTO.BreedId).Value);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                volonteer.AddPet(pet);

                await _unitOfWork.SaveChangesAsync();

                transaction.Commit();

                return pet.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex.Message);
                return Errors.General.ValueIsInvalid().ToErrorList();
            }
        }
    }
}
