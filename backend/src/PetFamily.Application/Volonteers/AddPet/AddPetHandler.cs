using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.AddPet
{
    public class AddPetHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddPetHandler> _logger;

        public AddPetHandler(
            IVolonteersRepository volonteersRepository,
            ISpeciesRepository speciesRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddPetHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handler(
            IValidator<AddPetCommand> validator,
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var speciesAndBreedResult = await _speciesRepository.IsSpeciesAndBreedExists(
                command.PetTypeDTO.SpeciesID,
                command.PetTypeDTO.BreedID);

            if (speciesAndBreedResult.IsFailure)
            {
                _logger.LogError(speciesAndBreedResult.Error.Message);
                return speciesAndBreedResult.Error.ToErrorList();
            }

            var volonteerResult = await _volonteersRepository.GetById(
                command.VolonteerID, cancellationToken);

            if(volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var volonteer = volonteerResult.Value;

            var donationDetailsCollection = new List<DonationDetails>();
            foreach (var donationDetail in command.DonationDetails)
                donationDetailsCollection.Add(DonationDetails.Create(
                    donationDetail.Name,
                    donationDetail.Link).
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
                    command.PetHealthInfoDTO.HelthInfo,
                    command.PetHealthInfoDTO.IsNeutered,
                    command.PetHealthInfoDTO.IsVaccinated).Value,
                donationDetailsCollection,
                PetType.Create(
                    command.PetTypeDTO.SpeciesID,
                    command.PetTypeDTO.BreedID).Value);

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
