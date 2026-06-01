using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Product.GetById;

public interface IGetProductByIdUseCase
{
    Task<ResponseProductJson> Execute(Guid id);
}