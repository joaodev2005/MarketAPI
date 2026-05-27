using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request);
}