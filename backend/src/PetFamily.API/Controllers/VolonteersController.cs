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

namespace PetFamily.API.Controllers
{
    public class VolonteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateVolonteerHandler handler,
            [FromServices] IValidator<CreateVolonteerRequest> validator,
            [FromBody] CreateVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
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
    }
}
