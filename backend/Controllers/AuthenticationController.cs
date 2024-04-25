using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Entities;
using QWiz.Helpers.Authentication;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Exception;

namespace QWiz.Controllers;

[Route("api/{v:apiVersion}/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[EnableCors("AllowOrigin")]
public class AuthenticationController(AuthenticationService authenticationService, IMapper mapper)
    : ControllerBase
{
    [HttpPost("Login")]
    [ProducesResponseType(typeof(AuthClaim), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        return Created(Request.Path.Value!, await authenticationService.Login(loginDto));
    }

    [HttpPost("Register")]
    [ProducesResponseType(typeof(AuthClaim), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] AppUserDto appUserDto)
    {
        var appUser = mapper.Map<AppUser>(appUserDto);
        return Created(Request.Path.Value!,
            await authenticationService.Register(appUser, appUserDto.Password, appUserDto.Roles));
    }


    [HttpGet("UserInfo")]
    [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status401Unauthorized)]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        return Ok(authenticationService.GetCurrentUser());
    }

    [HttpGet("Roles/{userId}")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetRolesByUserId(string userId)
    {
        return Ok(await authenticationService.GetRolesByUserId(userId));
    }

    [HttpPatch("LockAccount/{id}")]
    [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "Admin")]
    public IActionResult LockAccount(string id)
    {
        return Ok(authenticationService.LockAccount(id));
    }
}