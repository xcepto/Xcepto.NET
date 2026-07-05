using Microsoft.AspNetCore.Mvc;
using Samples.SSR.GUI.Requests;

namespace Samples.SSR.GUI.Controllers;

[Route("validate")]
public class PromiseValidationController: Controller
{
    [HttpPost]
    public IActionResult Index(ValidationRequest validationRequest)
    {
        if (validationRequest.Content.Contains("Welcome"))
            return new StatusCodeResult(200);
        return new StatusCodeResult(400);
    }
}