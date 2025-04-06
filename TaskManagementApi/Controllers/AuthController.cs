using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Microsoft.Extensions.Logging;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(User user)
    {
        _logger.LogInformation("Sign-up attempt for user: {Email}", user.Email);

        try
        {
            await _authService.SignUpAsync(user.Email, user.Password);
            _logger.LogInformation("Sign-up successful for user: {Email}", user.Email);
            return Ok("Sign-up successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sign-up failed for user: {Email}", user.Email);
            return StatusCode(500, "Sign-up failed");
        }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Signin(User user)
    {
        _logger.LogInformation("Sign-in attempt for user: {Email}", user.Email);

        try
        {
            var result = await _authService.SignInAsync(user.Email, user.Password);
            if (result == null)
            {
                _logger.LogWarning("Sign-in failed for user: {Email} - Invalid credentials or challenge", user.Email);
                return Unauthorized("Invalid credentials");
            }

            _logger.LogInformation("Sign-in successful for user: {Email}", user.Email);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sign-in failed for user: {Email}", user.Email);
            return StatusCode(500, "Sign-in failed");
        }
    }
}
