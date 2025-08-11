using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Species.Queries.GetBreedById;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Species.Application.Commands.AddBreed;
using PetFamily.Species.Application.Commands.AddSpecies;
using PetFamily.Species.Application.Commands.RemoveBreed;
using PetFamily.Species.Application.Commands.RemoveSpecies;
using PetFamily.Species.Application.Queries.GetAllSpecies;
using PetFamily.Species.Requests;

namespace PetFamily.Species.Presentation
{
    public class SpeciesController : ApplicationController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] IQueryHandler<IEnumerable<SpeciesDto>, GetAllSpeciesQuery> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new GetAllSpeciesQuery(), cancellationToken);
            return Ok(Envelope.Ok(result));
        }

        [HttpGet("breed")]
        public async Task<IActionResult> GetBreedById(
            [FromQuery] GetBreedByIdRequest request,
            [FromServices] IQueryHandler<BreedDto, GetBreedByIdQuery> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToQuery(), cancellationToken);
            return Ok(Envelope.Ok(result));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveSpecies(
            [FromBody] RemoveSpeciesByIdRequest request,
            [FromServices] ICommandHandler<RemoveSpeciesByIdCommand> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(),
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();
            return Ok(Envelope.Ok());
        }

        [HttpDelete("breed")]
        public async Task<IActionResult> RemoveBreed(
            [FromBody] RemoveBreedByIdRequest request,
            [FromServices] ICommandHandler<RemoveBreedByIdCommand> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(),
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();
            return Ok(Envelope.Ok());
        }

        [HttpPost("species")]
        public async Task<IActionResult> AddSpecies(
            [FromBody] AddSpeciesRequest request,
            [FromServices] ICommandHandler<Guid, AddSpeciesCommand> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(),
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{speciesId:guid}/breed")]
        public async Task<IActionResult> AddBreed(
            [FromRoute] Guid speciesId,
            [FromBody] AddBreedRequest request,
            [FromServices] ICommandHandler<Guid, AddBreedCommand> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(speciesId),
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();
            return Ok(Envelope.Ok(result.Value));
        }
    }
}
