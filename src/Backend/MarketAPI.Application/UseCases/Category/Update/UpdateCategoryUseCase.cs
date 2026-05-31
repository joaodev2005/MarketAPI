using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Repositories;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Category.Update;

public class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestCategoryJson request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            throw new NotFoundException("Category not found");

        var validator = new UpdateCategoryValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }

        category.Name = request.Name;

        await _unitOfWork.Commit();    
    }
}