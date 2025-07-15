using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PetFamily.Application.Volonteers.Create
{
    public class CreateVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateVolonteerHandler> _logger;

        public CreateVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            IValidator<CreateVolonteerCommand> validator, 
            CreateVolonteerCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return validationResult.ToErrorList();

            var volonteerId = Guid.NewGuid();

            var personalData = PersonalData.Create(
                command.PersonalDataDTO.FullName,
                command.PersonalDataDTO.Email,
                command.PersonalDataDTO.PhoneNumber)
                .Value;

            var volonteerByEmail = await _volonteersRepository.GetByEmail(command.PersonalDataDTO.FullName, cancellationToken);
            if (volonteerByEmail.IsSuccess)
                return Errors.Volonteer
                    .AlreadyExists()
                    .ToErrorList();

            var professionalData = ProfessionalData.Create(
                command.ProfessionalDataDTO.Description,
                command.ProfessionalDataDTO.ExperienceInYears)
                .Value;

            var socialNetworks = new List<SocialNetwork>();

            foreach (var sn in command.SocialNetworks)
                socialNetworks.Add(SocialNetwork.Create(sn.Name, sn.Link).Value);

            var donationDetails = new List<DonationDetails>();

            foreach (var dd in command.DonationDetails)
                donationDetails.Add(DonationDetails.Create(dd.Name, dd.Link).Value);

            var volonteer = new Volonteer(volonteerId,
                personalData,
                professionalData,
                new List<Pet>(),
                new SocialNetwokrsWrapper(socialNetworks),
                new DonationDetailsWrapper(donationDetails));

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {              
                await _volonteersRepository.Add(volonteer);

                transaction.Commit();

                _logger.LogInformation("Added volonteer {volonteer} with id {volonteerId}", volonteer, volonteerId);

                return volonteerId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while creating volonteer");
                return Errors.General
                    .ValueIsInvalid("volonteer creation")
                    .ToErrorList();
            }
        }
    }
}
