using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Category.Delete;

public class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryUseCase(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork, 
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task Execute(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            throw new NotFoundException("Category not found");
        
        var hasProducts = await _productRepository.ExistsByCategoryIdAsync(id);
        if (hasProducts)
            throw new ConflictException("Cannot delete category with products");

        await _categoryRepository.DeleteAsync(category);
        await _unitOfWork.Commit();
    }
}