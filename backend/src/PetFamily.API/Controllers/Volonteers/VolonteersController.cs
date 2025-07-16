using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.UpdateMainInfo;
using PetFamily.Application.Volonteers.UpdateSocialNetworks;
using PetFamily.Application.Volonteers.UpdateDonationDetails;
using PetFamily.Application.Volonteers.Delete;
using PetFamily.Application.Volonteers.AddPet;
using PetFamily.Application.Volonteers.AddPetPhotos;
using PetFamily.API.Processors;
using PetFamily.Domain.Shared;
using PetFamily.Application.Volonteers.RemovePetPhotos;
using PetFamily.Application.Volonteers.ShiftPetPosition;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;
using PetFamily.API.Controllers.Volonteers.Requests;

namespace PetFamily.API.Controllers.Volonteers
{
    public class VolonteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateVolonteerHandler handler,
            [FromBody] CreateVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(), 
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoRequest request,
            [FromServices] UpdateMainInfoHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(id), 
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/social-networks")]
        public async Task<ActionResult> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromBody] UpdateSocialNetworkRequest request,
            [FromServices] UpdateSocialNetworksHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(id), 
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/donation-details")]
        public async Task<ActionResult> UpdateDonationDetails(
            [FromRoute] Guid id,
            [FromBody] UpdateDonationDetailsRequest request,
            [FromServices] UpdateDonationDetailsHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(id), 
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{id:guid}/soft")]
        public async Task<ActionResult> SoftDelete(
            [FromRoute] Guid id,
            [FromServices] SoftDeleteVolonteerHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                new DeleteVolonteerCommand(id), 
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] HardDeleteVolonteerHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                new DeleteVolonteerCommand(id), 
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/{petId:guid}/{photos}")]
        public async Task<IActionResult> AddPhotos(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromRoute] string photos,
            [FromServices] AddPetPhotosHandler handler,
            IFormFileCollection files,
            CancellationToken cancellationToken = default)
        {
            List<Stream> streamCollection;
            await using var fileProsessor = new FormFileProcessor();
            streamCollection = fileProsessor.Process(files);

            var result = await handler.Handle(
                new AddPetPhotosCommand(
                volunteerId, petId, photos, streamCollection), 
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{id:guid}/pet")]
        public async Task<ActionResult> AddPet(
            [FromRoute] Guid id,
            [FromServices] AddPetHandler handler,
            [FromBody] AddPetRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handler(
                request.ToCommand(id), 
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{volonteerId:guid}/{petId:guid}/{photos}")]
        public async Task<IActionResult> RemoveFiles(
            [FromRoute] Guid volonteerId,
            [FromRoute] Guid petId,
            [FromRoute] string photos,
            [FromBody] RemovePetPhotoRequest request,
            [FromServices] RemovePetPhotosHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                new RemovePetPhotosCommand(
                volonteerId,
                petId,
                request.Paths.Select(
                    filePath => new FileInfo(
                        photos, FilePath.Create(filePath).Value))), 
                cancellationToken);
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
            [FromBody] ShiftPetPositionRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                new ShiftPetPositionCommand(
                volonteerId,
                petId,
                request.NewPosition), 
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }
    }
}
