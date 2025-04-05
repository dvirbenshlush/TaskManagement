using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task Signup(User user)
    {
        await _authService.SignUpAsync(user.Email, user.Password);
    }


    [HttpPost("signin")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> Signin(User user)
    {
        var res = await _authService.SignInAsync(user.Email, user.Password);
        return Ok(res);
    }
}

