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
            [FromServices] IValidator<CreateVolonteerCommand> validator,
            [FromBody] CreateVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator,
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
            [FromServices] IValidator<UpdateMainInfoCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(validator, 
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
            [FromServices] IValidator<UpdateSocialNetworksCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator,
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
            [FromServices] IValidator<UpdateDonationDetailsCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator,
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
            [FromServices] IValidator<DeleteVolonteerCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator, 
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
            [FromServices] IValidator<DeleteVolonteerCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator,
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
            [FromServices] IValidator<AddPetPhotosCommand> validator,
            IFormFileCollection files,
            CancellationToken cancellationToken = default)
        {
            List<Stream> streamCollection;
            await using var fileProsessor = new FormFileProcessor();
            streamCollection = fileProsessor.Process(files);

            var result = await handler.Handle(
                validator,
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
            [FromServices] IValidator<AddPetCommand> validator,
            [FromBody] AddPetRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handler(
                validator,
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
            [FromServices] IValidator<RemovePetPhotosCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator,
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
            [FromServices] IValidator<ShiftPetPositionCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                validator,
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
