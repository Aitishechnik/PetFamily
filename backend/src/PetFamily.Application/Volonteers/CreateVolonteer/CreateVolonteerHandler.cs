using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public class CreateVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<CreateVolonteerHandler> _logger;

        public CreateVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            ILogger<CreateVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolonteerDTO request, CancellationToken cancellationToken = default)
        {

            var volonteerId = Guid.NewGuid();

            var personalData = PersonalData.Create(
                request.FullName, 
                request.Email, 
                request.PhoneNumber)
                .Value;

            var volonteerByEmail = await _volonteersRepository.GetByEmail(request.Email);
            if(volonteerByEmail.IsSuccess)
                return Errors.Volonteer.AlreadyExists();

            var professionalData = ProfessionalData.Create(
                request.Description, 
                request.ExperienceInYears)
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

            await _volonteersRepository.Add(volonteer, cancellationToken);

            _logger.LogInformation("Added volonteer {volonteer} with id {volonteerId}", volonteer, volonteerId);

            return volonteerId;
        }
    }
}
