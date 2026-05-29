using MarketAPI.Application.UseCases.Category.Create;
using MarketAPI.Application.UseCases.Login;
using MarketAPI.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace MarketAPI.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
    }
}