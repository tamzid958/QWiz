using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;
using QWiz.Helpers.Exception;
using QWiz.Services;

namespace QWiz.Controllers;

[Route("api/{v:apiVersion}/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[EnableCors("AllowOrigin")]
[Authorize]
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
    public IActionResult Post([FromBody] CategoryWithReviewer categoryWithReviewer)
    {
        if (!ModelState.IsValid) return BadRequest();

        return Created(Request.Path.Value!, categoryService.Create(categoryWithReviewer));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, [FromBody] CategoryWithReviewer categoryWithReviewer)
    {
        if (!ModelState.IsValid) return BadRequest();

        return Ok(categoryService.Update(id, categoryWithReviewer));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionMessage), StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        categoryService.Delete(id);
        return NoContent();
    }
}