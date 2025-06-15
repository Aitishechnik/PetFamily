using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;

        public UpdateSocialNetworksHandler(
            IVolonteersRepository volonteersRepository,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateSocialNetworksRequest request,
            CancellationToken cancellationToken = default)
        {
            var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId, cancellationToken);

            if (volonteerResult.IsFailure)
                return volonteerResult.Error;

            var socialNetworks = new List<SocialNetwork>();

            foreach (var sn in request.SocialNetworks)
                socialNetworks.Add(SocialNetwork.Create(sn.Name, sn.Link).Value);

            volonteerResult.Value.UpdateSocialNetworks(new SocialNetwokrsWrapper(socialNetworks));

            var result = await _volonteersRepository.Save(volonteerResult.Value, cancellationToken);

            _logger.LogInformation("Volonteer with id {result} has been updated with new social networks", result);

            return result;
        }
    }
}
