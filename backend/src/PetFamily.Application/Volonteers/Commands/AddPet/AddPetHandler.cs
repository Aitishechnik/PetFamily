using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.AddPet
{
    public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetHandler> _logger;

        public AddPetHandler(
            IVolonteersRepository volonteersRepository,
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
            IUnitOfWork unitOfWork,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
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

            var isSpeciesAndBreedExists = await _readDbContext.Breeds.FirstOrDefaultAsync(
                b => b.Id == command.PetTypeDTO.BreedId &&
                     b.SpeciesId == command.PetTypeDTO.SpeciesId);

            if (isSpeciesAndBreedExists is null)
            {
                _logger.LogError("Specied or Breed was not found");
                return Errors.General.ValueIsInvalid(
                    "Species or Breed was not found").ToErrorList();
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
                    command.PetHealthInfoDTO.HelthInfo,
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
