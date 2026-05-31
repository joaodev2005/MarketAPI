using MarketAPI.Communication.Requests;

namespace MarketAPI.Application.UseCases.Category.Update;

public interface IUpdateCategoryUseCase
{
    Task Execute(Guid id, RequestCategoryJson request);
}