using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Categories.Read)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Categories.Read)]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Categories.Create)]
        public async Task<ActionResult<int>> Create([FromBody] CategoryDto categoryDto)
        {
            var id = await _mediator.Send(new CreateCategoryCommand(categoryDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Categories.Update)]
        public async Task<IActionResult> Update([FromBody] CategoryDto categoryDto)
        {
            await _mediator.Send(new UpdateCategoryCommand(categoryDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Categories.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}