namespace MarketAPI.Application.UseCases.Product.Delete;

public interface IDeleteProductUseCase
{
    Task Execute(Guid id);
}