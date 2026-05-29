using MarketAPI.Communication.Requests;

namespace MarketAPI.Application.UseCases.Category.Create;

public interface ICreateCategoryUseCase
{
    Task Execute(RequestCategoryJson request);
}