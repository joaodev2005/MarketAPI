using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Product.Create;

public interface ICreateProductUseCase
{
    Task<ResponseProductJson> Execute(RequestProductJson request);
}