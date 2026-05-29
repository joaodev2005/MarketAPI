using MarketAPI.Application.UseCases.Category.Create;
using MarketAPI.Communication.Requests;
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateCategoryUseCase useCase,
        [FromBody] RequestCategoryJson request)
    {
        await useCase.Execute(request);
        return Created();
    }
}