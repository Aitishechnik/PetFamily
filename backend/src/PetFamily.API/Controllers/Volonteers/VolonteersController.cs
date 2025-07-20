using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.API.Processors;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;
using PetFamily.API.Controllers.Volonteers.Requests;
using PetFamily.Application.Volonteers.Commands.AddPet;
using PetFamily.Application.Volonteers.Commands.ShiftPetPosition;
using PetFamily.Application.Volonteers.Commands.UpdateDonationDetails;
using PetFamily.Application.Volonteers.Commands.AddPetPhotos;
using PetFamily.Application.Volonteers.Commands.RemovePetPhotos;
using PetFamily.Application.Volonteers.Commands.UpdateSocialNetworks;
using PetFamily.Application.Volonteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volonteers.Commands.Delete;
using PetFamily.Application.Volonteers.Commands.Create;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Models;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volonteers.Queries.GetVolonteers;
using PetFamily.Application.Volonteers.GetById;

namespace PetFamily.API.Controllers.Volonteers
{
    public class VolonteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateVolonteerRequest request,
            [FromServices] ICommandHandler<Guid, CreateVolonteerCommand> handler,
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
            [FromServices] ICommandHandler<Guid, UpdateMainInfoCommand> handler,
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
            [FromServices] ICommandHandler<Guid, UpdateSocialNetworksCommand> handler,
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
            [FromServices] ICommandHandler<Guid, UpdateDonationDetailsCommand> handler,
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
            [FromServices] ICommandHandler<Guid, DeleteVolonteerCommand> handler,
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
            [FromServices] ICommandHandler<Guid, DeleteVolonteerCommand> handler,
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
            [FromServices] ICommandHandler<IReadOnlyList<FilePath>, AddPetPhotosCommand> handler,
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
            [FromBody] AddPetRequest request,
            [FromServices] ICommandHandler<Guid, AddPetCommand> handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
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
            [FromServices] ICommandHandler<RemovePetPhotosCommand> handler,
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
            [FromBody] ShiftPetPositionRequest request,
            [FromServices] ICommandHandler<ShiftPetPositionCommand> handler,
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

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetVolonteerWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<VolonteerDto>,
                GetVolonteersWithPaginationQuery> handler,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery();

            var respone = await handler.Handle(query, cancellationToken);

            return Ok(respone);
        }

        [HttpGet("/dapper")]
        public async Task<IActionResult> Get(
            [FromQuery] GetVolonteerWithPaginationRequest request,
            [FromServices] GetVolonteersWithPaginationHandlerDapper handler,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery();

            var respone = await handler.Handle(query, cancellationToken);

            return Ok(respone);
        }

        [HttpGet("{volonteerId:guid}")]
        public async Task<IActionResult> VolonteerById(
            [FromRoute] Guid volonteerId,
            [FromServices] IQueryHandler<VolonteerDto, 
                GetVolonteerByIdQuery> handler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVolonteerByIdRequest(volonteerId).ToQuery();

            var response = await handler.Handle(query, cancellationToken);

            return Ok(response);
        }
    }
}
