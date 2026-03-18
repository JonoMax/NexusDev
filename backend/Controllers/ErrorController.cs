using Microsoft.AspNetCore.Mvc;

namespace NexusDev.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        public IActionResult HandleError()
        {
            return Problem(
                detail: "An unexpected error occurred.",
                statusCode: 500
            );
        }
    }
}
