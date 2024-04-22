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
public class ApproverController(ApproverService approverService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Approver>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] ApproverQueries approverQueries)
    {
        return Ok(approverService.Get(approverQueries));
    }
}