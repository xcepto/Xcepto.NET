using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samples.SSR.GUI.Requests;
using Samples.SSR.GUI.ViewModels;

namespace Samples.SSR.GUI.Controllers;

[Route("path")]
public class PromisedPathController: Controller
{

    [HttpGet("get/get")]
    public IActionResult GetGet()
    {
        return View("Response", new PromisedPathViewModel("/path/validate/get"));
    }
    [HttpGet("validate/get")]
    public IActionResult ValidateGet()
    {
        return new StatusCodeResult(204);
    }
    
    [HttpGet("get/post")]
    public IActionResult GetPost()
    {
        return View("Response", new PromisedPathViewModel("/path/validate/post"));
    }
    [HttpPost("validate/post")]
    public IActionResult ValidatePost()
    {
        return new StatusCodeResult(204);
    }
    
    [HttpGet("get/patch")]
    public IActionResult GetPatch()
    {
        return View("Response", new PromisedPathViewModel("/path/validate/patch"));
    }
    [HttpPatch("validate/patch")]
    public IActionResult ValidatePatch()
    {
        return new StatusCodeResult(204);
    }
    
        
    [HttpGet("get/put")]
    public IActionResult GetPut()
    {
        return View("Response", new PromisedPathViewModel("/path/validate/put"));
    }
    [HttpPut("validate/put")]
    public IActionResult ValidatePut()
    {
        return new StatusCodeResult(204);
    }
    
    [HttpGet("get/delete")]
    public IActionResult GetDelete()
    {
        return View("Response", new PromisedPathViewModel("/path/validate/delete"));
    }
    [HttpDelete("validate/delete")]
    public IActionResult ValidateDelete()
    {
        return new StatusCodeResult(204);
    }
}