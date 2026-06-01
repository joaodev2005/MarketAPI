using MarketAPI.Communication.Requests;

namespace MarketAPI.Application.UseCases.Product.Update;

public interface IUpdateProductUseCase
{
    Task Execute(Guid id, RequestProductJson request);
}