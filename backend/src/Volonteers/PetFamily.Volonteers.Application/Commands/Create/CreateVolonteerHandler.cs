using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.Entities;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.Create
{
    public class CreateVolonteerHandler : ICommandHandler<Guid, CreateVolonteerCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateVolonteerCommand> _validator;
        private readonly ILogger<CreateVolonteerHandler> _logger;

        public CreateVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateVolonteerCommand> validator,
            ILogger<CreateVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            CreateVolonteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

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
                donationDetails.Add(DonationDetails.Create(dd.Name, dd.Description).Value);

            var volonteer = new Volonteer(volonteerId,
                personalData,
                professionalData,
                new List<Pet>(),
                socialNetworks,
                donationDetails);

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
