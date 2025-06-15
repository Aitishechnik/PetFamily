using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolonteersRepository volonteersRepository,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateMainInfoRequest request, 
            CancellationToken cancellationToken = default)
        {
            var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId, cancellationToken);
            if(volonteerResult.IsFailure)
                return volonteerResult.Error;

            var personalData = PersonalData.Create(
                request.MainInfoDTO.PersonalDataDTO.FullName,
                request.MainInfoDTO.PersonalDataDTO.Email,
                request.MainInfoDTO.PersonalDataDTO.PhoneNumber);

            var professionalData = ProfessionalData.Create(
                request.MainInfoDTO.ProfessionalDataDTO.Description,
                request.MainInfoDTO.ProfessionalDataDTO.ExperienceInYears);

            volonteerResult.Value.UpdateMainInfo(
                personalData.Value,
                professionalData.Value);

            var result = await _volonteersRepository.Save(volonteerResult.Value, cancellationToken);

            _logger.LogInformation("Volonteer with id {result} has been updated", result);

            return result;
        }
    }
}
