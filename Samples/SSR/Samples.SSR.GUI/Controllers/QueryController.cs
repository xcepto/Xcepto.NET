using Microsoft.AspNetCore.Mvc;

namespace Samples.SSR.GUI.Controllers;

[Route("query")]
public class QueryController: Controller
{
    [HttpGet]
    public IActionResult Index([FromQuery] int page)
    {
        if (page == 1)
            return View("Page1");
        if (page == 2)
            return View("Page2");
        throw new ArgumentException($"page {page} not found");
    }
}