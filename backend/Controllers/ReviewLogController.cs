using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Exception;
using QWiz.Helpers.HttpQueries;
using QWiz.Services;

namespace QWiz.Controllers;

[Route("api/{v:apiVersion}/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[EnableCors("AllowOrigin")]
[Authorize]
public class ReviewLogController(ReviewerLogService reviewerLogService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ReviewLog>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] ReviewLogQueries reviewLogQueries)
    {
        return Ok(reviewerLogService.Get(Request, reviewLogQueries));
    }

    [Authorize(Roles = "Reviewer,Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(ReviewLog), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ReviewLogDto reviewLog)
    {
        return Created(Request.Path.Value!, reviewerLogService.Create(reviewLog));
    }
}