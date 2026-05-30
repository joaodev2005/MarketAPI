using MarketAPI.Application.UseCases.Category.Create;
using MarketAPI.Application.UseCases.Category.List;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateCategoryUseCase useCase,
        [FromBody] RequestCategoryJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<ResponseCategoryJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromServices] IListCategoriesUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
}