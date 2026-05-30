using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Category.Create;

public interface ICreateCategoryUseCase
{
    Task<ResponseCategoryJson> Execute(RequestCategoryJson request);
}