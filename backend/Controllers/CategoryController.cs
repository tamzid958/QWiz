using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Exception;
using QWiz.Services;

namespace QWiz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[EnableCors("AllowOrigin")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(categoryService.Get(Request));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        return Ok(categoryService.GetById(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Post([FromBody] CategoryWithApprover categoryWithApprover)
    {
        if (!ModelState.IsValid) return BadRequest();

        return Created(Request.Path.Value!, categoryService.Create(categoryWithApprover));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, [FromBody] CategoryWithApprover categoryWithApprover)
    {
        if (!ModelState.IsValid) return BadRequest();

        return Ok(categoryService.Update(id, categoryWithApprover));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Delete(int id)
    {
        categoryService.Delete(id);
        return NoContent();
    }
}