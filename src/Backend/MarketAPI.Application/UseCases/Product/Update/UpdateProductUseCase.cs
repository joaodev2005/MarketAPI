using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Product.Update;

public class UpdateProductUseCase : IUpdateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductUseCase(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
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