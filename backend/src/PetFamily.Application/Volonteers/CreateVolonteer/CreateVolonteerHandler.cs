using System.Text.Json;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public class CreateVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;

        public CreateVolonteerHandler(
            IVolonteersRepository volonteersRepository)
        {
            _volonteersRepository = volonteersRepository;
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolonteerRequest request, CancellationToken cancellationToken = default)
        {

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

            var socialNetworks = new List<SocialNetwork>();
            
            foreach (var sn in request.socialNetworks)
            {
                var jsonDoc = JsonDocument.Parse(sn);
                var root = jsonDoc.RootElement;
                var name = root.GetProperty("name").GetString();
                var link = root.GetProperty("link").GetString();

                var socialNetworkResult = SocialNetwork.Create(name!, link!);
                if (socialNetworkResult.IsSuccess)
                    socialNetworks.Add(socialNetworkResult.Value);
            }

            var donationDetails = new List<DonationDetails>();

            foreach (var dd in request.donationDetails)
            {
                var jsonDoc = JsonDocument.Parse(dd);
                var root = jsonDoc.RootElement;
                var name = root.GetProperty("name").GetString();
                var description = root.GetProperty("description").GetString();
                var donationDetailsResult = DonationDetails.Create(name!, description!);
                if (donationDetailsResult.IsSuccess)
                    donationDetails.Add(donationDetailsResult.Value);
            }

            var volonteer = new Volonteer(volonteerId,
                personalData,
                professionalData,
                new List<Pet>(),
                new SocialNetwokrsWrapper(socialNetworks),
                new DonationDetailsWrapper(donationDetails));

            await _volonteersRepository.Add(volonteer);

            return volonteerId;
        }
    }
}
