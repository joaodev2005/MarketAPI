using MarketAPI.Application.UseCases.Login;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginWithEmailAndPasswordUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}