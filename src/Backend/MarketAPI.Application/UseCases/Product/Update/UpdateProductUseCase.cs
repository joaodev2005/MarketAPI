using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Constants;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketAPI.Application.UseCases.Product.Update;

public class UpdateProductUseCase : IUpdateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;

    public UpdateProductUseCase(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork, 
        IDistributedCache cache)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Execute(Guid id, RequestProductJson request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found");

        await ValidateAndThrowOnFailures(request);

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        await _unitOfWork.Commit();
        
        await _cache.RemoveAsync(CacheKeys.Products);
    }
    
    private async Task ValidateAndThrowOnFailures(RequestProductJson request)
    {
        var validator = new UpdateProductValidator();
        var result = await validator.ValidateAsync(request);

        var categoryExists = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (categoryExists is null)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                string.Empty, "Category not found"));

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}