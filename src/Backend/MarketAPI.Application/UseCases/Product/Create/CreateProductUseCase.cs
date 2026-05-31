using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Product.Create;

public class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductUseCase(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseProductJson> Execute(RequestProductJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var product = new Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);
        await _unitOfWork.Commit();

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

        return new ResponseProductJson
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryName = category!.Name
        };
    }
    
    private async Task ValidateAndThrowOnFailures(RequestProductJson request)
    {
        var validator = new CreateProductValidator();
        var result = await validator.ValidateAsync(request);

        var categoryExists = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (categoryExists is null)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                string.Empty, "Category not found"));

        if (result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}