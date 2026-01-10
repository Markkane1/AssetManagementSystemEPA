using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _mediator.Send(new RegisterCommand(registerDto));
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _mediator.Send(new LoginQuery(loginDto));
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserInfoDto>> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetCurrentUserQuery(userId));
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var deviceInfo = Request.Headers["User-Agent"].ToString();

        var result = await _mediator.Send(new RefreshTokenCommand(request.AccessToken, request.RefreshToken, deviceInfo, ipAddress));
        return Ok(result);
    }
}

public record RefreshTokenRequest(string AccessToken, string RefreshToken);
