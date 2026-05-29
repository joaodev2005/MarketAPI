using FluentValidation.Results;
using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Repositories;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Category.Create;

public class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryUseCase(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestCategoryJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var category = new Domain.Entities.Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.Commit();
    }
    
    private async Task ValidateAndThrowOnFailures(RequestCategoryJson request)
    {
        var validator = new CreateCategoryValidator();
        var result = await validator.ValidateAsync(request);

        var nameExists = await _categoryRepository.ExistsByNameAsync(request.Name);
        if (nameExists)
            result.Errors.Add(new ValidationFailure(string.Empty, "Category name already exists"));

        if (result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}