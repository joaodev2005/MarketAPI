using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Product.List;

public interface IListProductsUseCase
{
    Task<List<ResponseProductJson>> Execute();
}