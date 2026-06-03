using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Order.Admin.GetAll;

public interface IGetAllOrdersUseCase
{
    Task<List<ResponseOrderJson>> Execute();
}