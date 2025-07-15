using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;

        public UpdateSocialNetworksHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateSocialNetworksRequest request,
            CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId, cancellationToken);

                if (volonteerResult.IsFailure)
                    return volonteerResult.Error;

                var socialNetworks = new List<SocialNetwork>();

                foreach (var sn in request.SocialNetworks)
                    socialNetworks.Add(SocialNetwork.Create(sn.Name, sn.Link).Value);

                volonteerResult.Value.UpdateSocialNetworks(new SocialNetwokrsWrapper(socialNetworks));

                await _unitOfWork.SaveChanges(cancellationToken);

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {Id} has been updated with new social networks", volonteerResult.Value.Id);

                return volonteerResult.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while updating social networks for volonteer with id {VolonteerId}", request.VolonteerId);
                return Errors.General.ValueIsInvalid("volonteer social networks update");
            }
        }
    }
}
