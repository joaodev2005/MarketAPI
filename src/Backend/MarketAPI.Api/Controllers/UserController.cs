using MarketAPI.Application.UseCases.User.Register;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUserAccountJson request, 
        [FromServices] IRegisterUserAccountUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
}