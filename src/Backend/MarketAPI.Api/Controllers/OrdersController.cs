using MarketAPI.Application.UseCases.Order.Create;
using MarketAPI.Application.UseCases.Order.GetById;
using MarketAPI.Application.UseCases.Order.List;
using MarketAPI.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(ResponseOrderJson), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromServices] ICreateOrderUseCase useCase)
    {
        var response = await useCase.Execute();
        return Created(string.Empty, response);
    }
    
    [HttpGet]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(List<ResponseOrderJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromServices] IListOrdersUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(ResponseOrderJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetOrderByIdUseCase useCase,
        [FromRoute] Guid id)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }
}