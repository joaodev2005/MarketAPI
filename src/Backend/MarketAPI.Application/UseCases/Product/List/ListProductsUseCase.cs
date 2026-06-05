using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Product;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MarketAPI.Application.UseCases.Product.List;

public class ListProductsUseCase : IListProductsUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IDistributedCache _cache;
    private const string CacheKey = "products";

    public ListProductsUseCase(IProductRepository productRepository, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<List<ResponseProductJson>> Execute()
    {
        var cached = await _cache.GetStringAsync(CacheKey);

        if (cached is not null)
            return JsonSerializer.Deserialize<List<ResponseProductJson>>(cached)!;

        var products = await _productRepository.GetAllAsync();

        var response = products.Select(p => new ResponseProductJson
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category.Name
        }).ToList();

        await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(response),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return response;
    }
}