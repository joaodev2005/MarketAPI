using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Order.Create;

public interface ICreateOrderUseCase
{
    Task<ResponseOrderJson> Execute();
}