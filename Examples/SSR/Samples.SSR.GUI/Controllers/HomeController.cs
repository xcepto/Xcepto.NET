using Microsoft.AspNetCore.Mvc;

namespace Samples.SSR.GUI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View("Welcome");
    }
}