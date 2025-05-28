using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public class CreateVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;

        public CreateVolonteerHandler(IVolonteersRepository volonteersRepository)
        {
            _volonteersRepository = volonteersRepository;
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolonteerRequest request, CancellationToken cancellationToken = default)
        {
            var volonteerId = Guid.NewGuid();

            var personalData = PersonalData.Create(request.fullName, request.email, request.phoneNumber);
            if(personalData.IsFailure)
                return personalData.Error;

            var volonteerByEmail = await _volonteersRepository.GetByEmail(request.email);
            if (volonteerByEmail.IsSuccess)
                return Errors.Volonteer.AlreadyExists();

            var professionalData = ProfessionalData.Create(request.description, request.experienceInYears);
            if(professionalData.IsFailure)
                return professionalData.Error;

            var socialNetworks = SocialNetwork.Create(request.socialNetworkName, request.socialNetworkLink);
            if(socialNetworks.IsFailure)
                return socialNetworks.Error;

            var donationDetails = DonationDetails.Create(request.donationDetailsName, request.donationDetailsDescription);
            if(donationDetails.IsFailure)
                return donationDetails.Error;

            var socialNetworksCollection = new List<SocialNetwork>();
            socialNetworksCollection!.Add(socialNetworks.Value);

            var donationDetailsCollection = new List<DonationDetails>();
            donationDetailsCollection!.Add(donationDetails.Value);

            var volonteer = new Volonteer(volonteerId,
                personalData.Value,
                professionalData.Value,
                new List<Pet>(),
                new SocialNetwokrsWrapper(socialNetworksCollection!),
                new DonationDetailsWrapper(donationDetailsCollection));

            await _volonteersRepository.Add(volonteer);

            return volonteerId;
        }
    }
}
