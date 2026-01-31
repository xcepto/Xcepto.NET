using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Samples.SSR.GUI.Requests;

namespace Samples.SSR.GUI.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _users;
    private readonly SignInManager<IdentityUser> _signIn;

    public AuthController(UserManager<IdentityUser> users, SignInManager<IdentityUser> signIn)
    {
        _users = users;
        _signIn = signIn;
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View("Register");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest dto)
    {
        var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };

        var result = await _users.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);

            return View("Register");
        }

        await _signIn.SignInAsync(user, true);

        return Redirect("/");
    }


    [HttpGet("login")]
    public IActionResult Login()
    {
        return View("Login");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest dto)
    {
        var result = await _signIn.PasswordSignInAsync(dto.Email, dto.Password, true, false);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                ModelState.AddModelError("", "Account is locked.");
            else if (result.IsNotAllowed)
                ModelState.AddModelError("", "Login not allowed.");
            else
                ModelState.AddModelError("", "Invalid email or password.");

            return View("Login");
        }

        return Redirect("/");
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
