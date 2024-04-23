using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Services;

namespace QWiz.Controllers;

[Route("api/{v:apiVersion}/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Authorize]
[EnableCors("AllowOrigin")]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(AnalyticsDto), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(analyticsService.Get());
    }
}