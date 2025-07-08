using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
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

        public async Task<Result<Guid, Error>> Handler(
            AddPetRequest addPetRequest,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransaction();

            try
            {
                var speciesAndBreedResult = await _speciesRepository.IsSpeciesAndBreedExists(
                    addPetRequest.PetTypeDTO.SpeciesID,
                    addPetRequest.PetTypeDTO.BreedID);

                if (speciesAndBreedResult.IsFailure)
                {
                    _logger.LogError(speciesAndBreedResult.Error.Message);
                    return speciesAndBreedResult.Error;
                }

                var volonteerResult = await _volonteersRepository.GetById(
                    addPetRequest.VolonteerID, cancellationToken);

                if(volonteerResult.IsFailure)
                    return volonteerResult.Error;

                var volonteer = volonteerResult.Value;

                var donationDetailsCollection = new List<DonationDetails>();
                foreach (var donationDetail in addPetRequest.DonationDetails)
                    donationDetailsCollection.Add(DonationDetails.Create(
                        donationDetail.Name,
                        donationDetail.Description).
                        Value);

                var pet = new Pet(
                    Guid.NewGuid(),
                    volonteer.Id,
                    PetGeneralInfo.Create(
                        addPetRequest.PetGeneralInfoDTO.Name,
                        addPetRequest.PetGeneralInfoDTO.Description,
                        addPetRequest.PetGeneralInfoDTO.Address,
                        addPetRequest.PetGeneralInfoDTO.OwnerPhoneNumber,
                        addPetRequest.PetGeneralInfoDTO.DateOfBirth,
                        addPetRequest.PetGeneralInfoDTO.HelpStatus).Value,
                    PetCharacteristics.Create(
                        addPetRequest.PetCharacteristicsDTO.Color,
                        addPetRequest.PetCharacteristicsDTO.Weight,
                        addPetRequest.PetCharacteristicsDTO.Height).Value,
                    PetHealthInfo.Create(
                        addPetRequest.PetHealthInfoDTO.HelthInfo,
                        addPetRequest.PetHealthInfoDTO.IsNeutered,
                        addPetRequest.PetHealthInfoDTO.IsVaccinated).Value,
                    donationDetailsCollection,
                    PetType.Create(
                        addPetRequest.PetTypeDTO.SpeciesID,
                        addPetRequest.PetTypeDTO.BreedID).Value);

                volonteer.AddPet(pet);
                _unitOfWork.EntryChangeStateOnAdded(pet);
                await _unitOfWork.SaveChanges();
                transaction.Commit();
                
                return pet.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex.Message);
                return Errors.General.ValueIsInvalid();
            }
        }
    }
}
