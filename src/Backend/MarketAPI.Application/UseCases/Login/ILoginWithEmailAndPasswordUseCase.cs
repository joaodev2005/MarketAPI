using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Login;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}