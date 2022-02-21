using FlagshipProductTest.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlagshipProductTest.Client.Controllers
{
    [ApiController]
    public class RootController : ControllerBase
    {
        protected IActionResult CreateResponse(ErrorResponse error, FaultMode category)
        {
            var code = category switch
            {
                FaultMode.UNAUTHORIZED => (int)HttpStatusCode.Unauthorized,
                FaultMode.CLIENT_INVALID_ARGUMENT => (int)HttpStatusCode.BadRequest,
                FaultMode.INVALID_OBJECT_STATE => (int)HttpStatusCode.Conflict,
                FaultMode.GATEWAY_ERROR => (int)HttpStatusCode.BadGateway,
                FaultMode.REQUESTED_ENTITY_NOT_FOUND => (int)HttpStatusCode.NotFound,
                FaultMode.LIMIT_EXCEEDED => Microsoft.AspNetCore.Http.StatusCodes.Status429TooManyRequests,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            return new ObjectResult(error)
            {
                StatusCode = code
            };
        }
    }
}
