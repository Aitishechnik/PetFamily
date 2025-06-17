using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Delete
{
    public class SoftDeleteVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<SoftDeleteVolonteerHandler> _logger;

        public SoftDeleteVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            ILogger<SoftDeleteVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            DeleteVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _volonteersRepository.GetById(request.VolonteerId);
            if(result.IsFailure)
                return result.Error;

            if (result.Value.IsDeleted)
                return Errors.General.AlreadyDeleted(request.VolonteerId);

            result.Value.Delete();

            await _volonteersRepository.Save(result.Value, cancellationToken);

            _logger.LogInformation("Volonteer with id {request.VolonteerId) was softly deleted}", request.VolonteerId);

            return result.Value.Id;
        }        
    }
}
