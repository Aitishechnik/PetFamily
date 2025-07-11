using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.UpdateMainInfo;
using PetFamily.Application.Volonteers.UpdateSocialNetworks;
using PetFamily.Contracts;
using PetFamily.Application.Volonteers.UpdateDonationDetails;
using PetFamily.Application.Volonteers.Delete;
using PetFamily.Application.Volonteers.AddPet;
using PetFamily.Application.Volonteers.AddPetPhotos;
using PetFamily.API.Processors;
using PetFamily.Domain.Shared;
using PetFamily.Application.Volonteers.RemovePetPhotos;
using PetFamily.Application.Volonteers.ShiftPetPosition;

namespace PetFamily.API.Controllers
{
    public class VolonteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateVolonteerHandler handler,
            [FromServices] IValidator<CreateVolonteerRequest> validator,
            [FromBody] VolonteerDTO dto,
            CancellationToken cancellationToken = default)
        {
            var request = new CreateVolonteerRequest(
                dto.PersonalDataDTO,
                dto.ProfessionalDataDTO,
                dto.SocialNetworks,
                dto.DonationDetails);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromBody] MainInfoDTO dto,
            [FromServices] UpdateMainInfoHandler handler,
            [FromServices] IValidator<UpdateMainInfoRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new UpdateMainInfoRequest(id, dto);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/social-networks")]
        public async Task<ActionResult> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromBody] IEnumerable<SocialNetworkDTO> dtos,
            [FromServices] UpdateSocialNetworksHandler handler,
            [FromServices] IValidator<UpdateSocialNetworksRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new UpdateSocialNetworksRequest(id, dtos);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/donation-details")]
        public async Task<ActionResult> UpdateDonationDetails(
            [FromRoute] Guid id,
            [FromBody] IEnumerable<DonationDetailsDTO> dtos,
            [FromServices] UpdateDonationDetailsHandler handler,
            [FromServices] IValidator<UpdateDonationDetailsRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new UpdateDonationDetailsRequest(id, dtos);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{id:guid}/soft")]
        public async Task<ActionResult> SoftDelete(
            [FromRoute] Guid id,
            [FromServices] SoftDeleteVolonteerHandler handler,
            [FromServices] IValidator<DeleteVolonteerRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteVolonteerRequest(id);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] HardDeleteVolonteerHandler handler,
            [FromServices] IValidator<DeleteVolonteerRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteVolonteerRequest(id);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/{petId:guid}/photos")]
        public async Task<IActionResult> AddPhotos(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] AddPetPhotosHandler handler,
            [FromServices] IValidator<AddPetPhotosRequest> validator,
            IFormFileCollection files,
            CancellationToken cancellationToken = default)
        {
            List<Stream> streamCollection;
            await using var fileProsessor = new FormFileProcessor();
            streamCollection = fileProsessor.Process(files);
            var request = new AddPetPhotosRequest(volunteerId, petId, streamCollection);

            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{id:guid}/pet")]
        public async Task<ActionResult> AddPet(
            [FromRoute] Guid id,
            [FromServices] AddPetHandler handler,
            [FromServices] IValidator<AddPetRequest> validator,
            [FromBody] PetDTO dto,
            CancellationToken cancellationToken = default)
        {
            var request = new AddPetRequest(
                id,
                dto.PetGeneralInfoDTO,
                dto.PetCharacteristicsDTO,
                dto.PetHealthInfoDTO,
                dto.DonationDetails,
                dto.PetTypeDTO);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handler(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{volonteerId:guid}/{petId:guid}/photos")]
        public async Task<IActionResult> RemoveFiles(
            [FromRoute] Guid volonteerId,
            [FromRoute] Guid petId,
            [FromBody] RemovePetPhotoDTO dto,
            [FromServices] RemovePetPhotosHandler handler,
            [FromServices] IValidator<RemovePetPhotosRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new RemovePetPhotosRequest(
                volonteerId, 
                petId, 
                dto.Paths.Select(filePath => FilePath.Create(filePath).Value));

            var validationResult = validator.Validate(request);

            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(Envelope.Ok());
        }

        [HttpPut("{volonteerId:guid}/{petId:guid}/shift-pet-position")]
        public async Task<IActionResult> ShiftPetPosition(
            [FromRoute] Guid volonteerId,
            [FromRoute] Guid petId,
            [FromServices] ShiftPetPositionHandler handler,
            [FromBody] ShiftPetPositionDTO dto,
            [FromServices] IValidator<ShiftPetPositionRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var request = new ShiftPetPositionRequest(
                volonteerId, 
                petId, 
                dto.NewPosition);

            var validationResult = validator.Validate(request);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }
    }
}
