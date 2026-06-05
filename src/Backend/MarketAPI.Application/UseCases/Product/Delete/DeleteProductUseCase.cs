using MarketAPI.Domain.Constants;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketAPI.Application.UseCases.Product.Delete;

public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;

    public DeleteProductUseCase(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork, 
        IDistributedCache cache)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Execute(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found");

        await _productRepository.DeleteAsync(product);
        await _unitOfWork.Commit();
        
        await _cache.RemoveAsync(CacheKeys.Products);
    }
}