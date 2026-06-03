using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Order.GetById;

public interface IGetOrderByIdUseCase
{
    Task<ResponseOrderJson> Execute(Guid id);
}