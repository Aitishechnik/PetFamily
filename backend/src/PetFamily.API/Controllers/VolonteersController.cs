using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volonteers.CreateVolonteer;

namespace PetFamily.API.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class VolonteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolonteerHandler handler,
            [FromBody] CreateVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
