using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories;

namespace MarketAPI.Application.UseCases.Category.List;

public class ListCategoriesUseCase : IListCategoriesUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategoriesUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ResponseCategoryJson>> Execute()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(c => new ResponseCategoryJson
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }
}