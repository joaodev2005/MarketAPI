using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Category.List;

public interface IListCategoriesUseCase
{
    Task<List<ResponseCategoryJson>> Execute();
}