using MarketAPI.Application.UseCases.Product.Create;
using MarketAPI.Application.UseCases.Product.List;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ResponseProductJson), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateProductUseCase useCase,
        [FromBody] RequestProductJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<ResponseProductJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromServices] IListProductsUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
}