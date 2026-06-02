using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Order.List;

public interface IListOrdersUseCase
{
    Task<List<ResponseOrderJson>> Execute();
}