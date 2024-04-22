using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Entities;
using QWiz.Helpers.HttpQueries;
using QWiz.Services;

namespace QWiz.Controllers;

[Route("api/{v:apiVersion}/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[EnableCors("AllowOrigin")]
[Authorize]
public class ReviewerController(ReviewerService reviewerService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Reviewer>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] ReviewerQueries reviewerQueries)
    {
        return Ok(reviewerService.Get(reviewerQueries));
    }
}