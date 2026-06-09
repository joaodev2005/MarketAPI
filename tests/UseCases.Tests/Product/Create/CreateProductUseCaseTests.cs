using CommonTestUtilities.Cache;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.Create;
using MarketAPI.Domain.Entities;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Product.Create;

public class CreateProductUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestProductJsonBuilder.Build();
        var category = new Category { Id = request.CategoryId, Name = "Test" };

        var useCase = CreateUseCase(category);
        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_When_Category_Not_Found()
    {
        var request = RequestProductJsonBuilder.Build();

        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_When_Name_Is_Empty()
    {
        var request = RequestProductJsonBuilder.Build();
        request.Name = "";

        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_When_Price_Is_Less_Than_or_Equal_to_0()
    {
        var request = RequestProductJsonBuilder.Build();
        request.Price = 0;

        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_When_Stock_Is_Negative()
    {
        var request = RequestProductJsonBuilder.Build();
        request.Stock = -1;
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(request);
        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    private CreateProductUseCase CreateUseCase(Category? category = null)
    {
        var categoryRepositoryBuilder = new ICategoryRepositoryBuilder();

        if (category is not null)
            categoryRepositoryBuilder.GetById(category);

        return new CreateProductUseCase(
            new IProductRepositoryBuilder().Build(),
            categoryRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build(),
            new IDistributedCacheBuilder().Build());
    }
}
