using MarketAPI.Application.UseCases.Cart.AddItem;
using MarketAPI.Application.UseCases.Cart.GetCart;
using MarketAPI.Application.UseCases.Cart.RemoveItem;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CartController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem(
        [FromServices] IAddItemToCartUseCase useCase,
        [FromBody] RequestAddItemToCartJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseCartJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart([FromServices] IGetCartUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
    
    [HttpDelete("{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(
        [FromServices] IRemoveItemFromCartUseCase useCase,
        [FromRoute] Guid itemId)
    {
        await useCase.Execute(itemId);
        return NoContent();
    }
}