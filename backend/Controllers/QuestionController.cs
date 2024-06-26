﻿using Microsoft.AspNetCore.Authorization;
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
[EnableCors("AllowOrigin")]
[Authorize]
public class QuestionController(QuestionService questionService) : ControllerBase
{
    [HttpGet("Types")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public IActionResult GetTypes()
    {
        return Ok(QuestionService.GetTypes());
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<List<Question>>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] QuestionQueries questionQueries)
    {
        return Ok(questionService.Get(Request, paginationFilter, questionQueries));
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        return Ok(questionService.GetById(id));
    }

    [Authorize(Roles = "QuestionSetter,Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(Question), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] QuestionDto question)
    {
        return Created(Request.Path.Value!, questionService.Create(question));
    }

    [Authorize(Roles = "QuestionSetter,Admin")]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Update(long id, [FromBody] QuestionDto question)
    {
        return Ok(questionService.Update(id, question));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("AddToBank/{id:long}/{accept:bool}")]
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult AddToQuestionBank(long id, bool accept)
    {
        return Ok(questionService.AddToQuestionBank(id, accept));
    }

    [Authorize(Roles = "QuestionSetter,Admin")]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        questionService.Delete(id);
        return NoContent();
    }
}