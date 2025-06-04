using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public class CreateVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IValidator<CreateVolonteerRequest> _validator;

        public CreateVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IValidator<CreateVolonteerRequest> validator)
        {
            _volonteersRepository = volonteersRepository;
            _validator = validator;
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolonteerRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var error = Error.Validation(
                    validationResult.Errors[0].ErrorCode,
                    validationResult.Errors[0].ErrorMessage);

                return error;
            }

            var volonteerId = Guid.NewGuid();

            var personalData = PersonalData.Create(
                request.fullName, 
                request.email, 
                request.phoneNumber)
                .Value;

            var volonteerByEmail = await _volonteersRepository.GetByEmail(request.email);

            var professionalData = ProfessionalData.Create(
                request.description, 
                request.experienceInYears)
                .Value;

            var socialNetworks = SocialNetwork.Create(
                request.socialNetworkName, 
                request.socialNetworkLink)
                .Value;

            var donationDetails = DonationDetails.Create(
                request.donationDetailsName, 
                request.donationDetailsDescription)
                .Value;

            var socialNetworksCollection = new List<SocialNetwork>();
            socialNetworksCollection!.Add(socialNetworks);

            var donationDetailsCollection = new List<DonationDetails>();
            donationDetailsCollection!.Add(donationDetails);

            var volonteer = new Volonteer(volonteerId,
                personalData,
                professionalData,
                new List<Pet>(),
                new SocialNetwokrsWrapper(socialNetworksCollection),
                new DonationDetailsWrapper(donationDetailsCollection));

            await _volonteersRepository.Add(volonteer);

            return volonteerId;
        }
    }
}
