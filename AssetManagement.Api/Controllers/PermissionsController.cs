using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PermissionDto>>> GetAll()
    {
        var permissions = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(permissions);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        var categories = await _mediator.Send(new GetPermissionCategoriesQuery());
        return Ok(categories);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<List<PermissionDto>>> GetByCategory(string category)
    {
        var permissions = await _mediator.Send(new GetPermissionsByCategoryQuery(category));
        return Ok(permissions);
    }
}
