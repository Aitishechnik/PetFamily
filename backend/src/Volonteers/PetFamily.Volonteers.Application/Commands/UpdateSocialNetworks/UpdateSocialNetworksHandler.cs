using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateSocialNetworksCommand> _validator;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;

        public UpdateSocialNetworksHandler(
            IVolonteersRepository volonteersRepository,
            [FromKeyedServices(Modules.Volonteers)] IUnitOfWork unitOfWork,
            IValidator<UpdateSocialNetworksCommand> validator,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateSocialNetworksCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId, cancellationToken);

            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var socialNetworks = new List<SocialNetwork>();

            foreach (var sn in command.SocialNetworks)
                socialNetworks.Add(SocialNetwork.Create(sn.Name, sn.Link).Value);

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                volonteerResult.Value.UpdateSocialNetworks(socialNetworks);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {Id} has been updated with new social networks", volonteerResult.Value.Id);

                return volonteerResult.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while updating social networks for volonteer with id {VolonteerId}", command.VolonteerId);
                return Errors.General
                    .ValueIsInvalid("volonteer social networks update")
                    .ToErrorList();
            }
        }
    }
}
