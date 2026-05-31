using MarketAPI.Domain.Repositories;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Category.Delete;

public class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            throw new NotFoundException("Category not found");

        await _categoryRepository.DeleteAsync(category);
        await _unitOfWork.Commit();
    }
}