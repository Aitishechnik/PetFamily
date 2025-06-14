using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using static PetFamily.Domain.Shared.Errors;

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
            var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId);
            if(volonteerResult.IsFailure)
                return volonteerResult.Error;

            var personalData = PersonalData.Create(
                request.Dto.PersonalDataDTO.FullName,
                request.Dto.PersonalDataDTO.Email,
                request.Dto.PersonalDataDTO.PhoneNumber);

            var professionalData = ProfessionalData.Create(
                request.Dto.ProfessionalDataDTO.Description,
                request.Dto.ProfessionalDataDTO.ExperienceInYears);

            volonteerResult.Value.UpdateMainInfo(
                personalData.Value,
                professionalData.Value);

            var result = await _volonteersRepository.Save(volonteerResult.Value, cancellationToken);

            _logger.LogInformation("Volonteer with id {result} has been updated", result);

            return result;
        }
    }
}
