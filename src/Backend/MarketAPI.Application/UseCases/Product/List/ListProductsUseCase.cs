using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Product;

namespace MarketAPI.Application.UseCases.Product.List;

public class ListProductsUseCase : IListProductsUseCase 
{
    private readonly IProductRepository _productRepository;

    public ListProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ResponseProductJson>> Execute()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(p => new ResponseProductJson
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category.Name
        }).ToList();
    }
}