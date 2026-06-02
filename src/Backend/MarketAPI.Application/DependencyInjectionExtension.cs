using MarketAPI.Application.UseCases.Cart.AddItem;
using MarketAPI.Application.UseCases.Cart.GetCart;
using MarketAPI.Application.UseCases.Category.Create;
using MarketAPI.Application.UseCases.Category.Delete;
using MarketAPI.Application.UseCases.Category.List;
using MarketAPI.Application.UseCases.Category.Update;
using MarketAPI.Application.UseCases.Login;
using MarketAPI.Application.UseCases.Product.Create;
using MarketAPI.Application.UseCases.Product.Delete;
using MarketAPI.Application.UseCases.Product.GetById;
using MarketAPI.Application.UseCases.Product.List;
using MarketAPI.Application.UseCases.Product.Update;
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
        services.AddScoped<IListCategoriesUseCase, ListCategoriesUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        services.AddScoped<IListProductsUseCase, ListProductsUseCase>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
        services.AddScoped<IAddItemToCartUseCase, AddItemToCartUseCase>();
        services.AddScoped<IGetCartUseCase, GetCartUseCase>();
    }
}