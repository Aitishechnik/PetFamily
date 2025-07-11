using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateMainInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId, cancellationToken);
                if (volonteerResult.IsFailure)
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

                await _unitOfWork.SaveChanges();

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {Id} has been updated", volonteerResult.Value.Id);

                return volonteerResult.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error occurred while updating main info for volonteer with id {VolonteerId}", request.VolonteerId);
                return Errors.General.ValueIsInvalid("volonteer main info update");
            }
        }
    }
}
