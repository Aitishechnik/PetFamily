using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Dtos;
using PetFamily.Application.Species.Commands.AddBreed;
using PetFamily.Application.Species.Commands.AddSpecies;
using PetFamily.Application.Species.Commands.RemoveBreed;
using PetFamily.Application.Species.Commands.RemoveSpecies;
using PetFamily.Application.Species.Queries.GetAllSpecies;
using PetFamily.Application.Species.Queries.GetBreedById;

namespace PetFamily.API.Controllers.Species
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
