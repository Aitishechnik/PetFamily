using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

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

        public async Task<Result<Guid, Error>> Handle(CreateVolonteerRequest request, CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var volonteerId = Guid.NewGuid();

                var personalData = PersonalData.Create(
                    request.PersonalDataDTO.FullName,
                    request.PersonalDataDTO.Email,
                    request.PersonalDataDTO.PhoneNumber)
                    .Value;

                var volonteerByEmail = await _volonteersRepository.GetByEmail(request.PersonalDataDTO.FullName, cancellationToken);
                if (volonteerByEmail.IsSuccess)
                    return Errors.Volonteer.AlreadyExists();

                var professionalData = ProfessionalData.Create(
                    request.ProfessionalDataDTO.Description,
                    request.ProfessionalDataDTO.ExperienceInYears)
                    .Value;

                var socialNetworks = new List<SocialNetwork>();

                foreach (var sn in request.SocialNetworks)
                    socialNetworks.Add(SocialNetwork.Create(sn.Name, sn.Link).Value);

                var donationDetails = new List<DonationDetails>();

                foreach (var dd in request.DonationDetails)
                    donationDetails.Add(DonationDetails.Create(dd.Name, dd.Description).Value);

                var volonteer = new Volonteer(volonteerId,
                    personalData,
                    professionalData,
                    new List<Pet>(),
                    new SocialNetwokrsWrapper(socialNetworks),
                    new DonationDetailsWrapper(donationDetails));
                
                await _unitOfWork.AddAsync(volonteer, cancellationToken);

                await _unitOfWork.SaveChanges();

                transaction.Commit();

                _logger.LogInformation("Added volonteer {volonteer} with id {volonteerId}", volonteer, volonteerId);

                return volonteerId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while creating volonteer");
                return Errors.General.ValueIsInvalid("volonteer creation");
            }
        }
    }
}
