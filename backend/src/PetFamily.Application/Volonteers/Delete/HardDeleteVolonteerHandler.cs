using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Delete
{
    public class HardDeleteVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<HardDeleteVolonteerHandler> _logger;

        public HardDeleteVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            ILogger<HardDeleteVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            DeleteVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _volonteersRepository.GetById(request.VolonteerId);
            if (result.IsFailure)
                return result.Error;

            await _volonteersRepository.Delete(result.Value, cancellationToken);

            _logger.LogInformation("Volonteer with id {request.VolonteerId) was permenantly deleted}", request.VolonteerId);

            return result.Value.Id;
        }
    }
}
