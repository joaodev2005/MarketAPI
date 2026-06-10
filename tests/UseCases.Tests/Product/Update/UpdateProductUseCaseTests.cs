using CommonTestUtilities.Cache;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.Update;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Product.Update;

public class UpdateProductUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestProductJsonBuilder.Build();

        var product = new MarketAPI.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            CategoryId = request.CategoryId
        };

        var category = new MarketAPI.Domain.Entities.Category { Id = request.CategoryId, Name = "Test" };

        var useCase = CreateUseCase(product, category);
        var act = async () => await useCase.Execute(product.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Product_Not_Found()
    {
        var request = RequestProductJsonBuilder.Build();
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var product = new MarketAPI.Domain.Entities.Product { Id = Guid.NewGuid() };
        var request = RequestProductJsonBuilder.Build();
        request.Name = "";

        var useCase = CreateUseCase(product);
        var act = async () => await useCase.Execute(product.Id, request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var product = new MarketAPI.Domain.Entities.Product { Id = Guid.NewGuid() };
        var request = RequestProductJsonBuilder.Build();

        var useCase = CreateUseCase(product);
        var act = async () => await useCase.Execute(product.Id, request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_Price_Is_Invalid()
    {
        var product = new MarketAPI.Domain.Entities.Product { Id = Guid.NewGuid() };
        var request = RequestProductJsonBuilder.Build();
        request.Price = -10;

        var useCase = CreateUseCase(product);
        var act = async () => await useCase.Execute(product.Id, request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    private UpdateProductUseCase CreateUseCase(
       MarketAPI.Domain.Entities.Product? product = null,
       MarketAPI.Domain.Entities.Category? category = null)
    {
        var productRepositoryBuilder = new IProductRepositoryBuilder();
        var categoryRepositoryBuilder = new ICategoryRepositoryBuilder();

        if (product is not null)
            productRepositoryBuilder.GetById(product);

        if (category is not null)
            categoryRepositoryBuilder.GetById(category);

        return new UpdateProductUseCase(
            productRepositoryBuilder.Build(),
            categoryRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build(),
            new IDistributedCacheBuilder().Build());
    }
}
