using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Exception;
using QWiz.Helpers.HttpQueries;
using QWiz.Helpers.Paginator;
using QWiz.Services;

namespace QWiz.Controllers;

[Route("api/{v:apiVersion}/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
[EnableCors("AllowOrigin")]
public class AppUserController(AppUserService appUserService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<List<AppUser>>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] AppUserQueries appUserQueries)
    {
        return Ok(appUserService.Get(paginationFilter, Request, appUserQueries));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status404NotFound)]
    public IActionResult GetById(string id)
    {
        return Ok(appUserService.GetById(id));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status401Unauthorized)]
    public IActionResult Update(string id, [FromBody] UpdateAppUserDto userDto)
    {
        return Ok(appUserService.Update(id, userDto));
    }
}