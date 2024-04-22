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
public class ApprovalLogController(ApprovalLogService approvalLogService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ApprovalLog>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] ApprovalLogQueries approvalLogQueries)
    {
        return Ok(approvalLogService.Get(Request, approvalLogQueries));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApprovalLog), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ApprovalLogDto approvalLog)
    {
        return Created(Request.Path.Value!, approvalLogService.Create(approvalLog));
    }
}