using MarketAPI.Application.UseCases.Cart.AddItem;
using MarketAPI.Communication.Requests;
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
}