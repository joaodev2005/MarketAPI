using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Product.GetById;

public class GetProductByIdUseCase : IGetProductByIdUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResponseProductJson> Execute(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found");

        return new ResponseProductJson
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryName = product.Category.Name
        };
    }
}