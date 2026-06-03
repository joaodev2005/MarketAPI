using MarketAPI.Communication.Requests;

namespace MarketAPI.Application.UseCases.Order.Admin.UpdateStatus;

public interface IUpdateOrderStatusUseCase
{
    Task Execute(Guid id, RequestUpdateOrderStatusJson request);
}