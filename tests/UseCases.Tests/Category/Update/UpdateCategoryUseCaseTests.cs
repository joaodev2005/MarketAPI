using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Category.Update;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Category.Update;

public class UpdateCategoryUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var category = new MarketAPI.Domain.Entities.Category { Id = Guid.NewGuid(), Name = "Old Name" };
        var request = RequestCategoryJsonBuilder.Build();

        var useCase = CreateUseCase(category);
        var act = async () => await useCase.Execute(category.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var request = RequestCategoryJsonBuilder.Build();
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var category = new MarketAPI.Domain.Entities.Category { Id = Guid.NewGuid(), Name = "Old Name" };
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = "";

        var useCase = CreateUseCase(category);
        var act = async () => await useCase.Execute(category.Id, request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    private UpdateCategoryUseCase CreateUseCase(MarketAPI.Domain.Entities.Category? category = null)
    {
        var categoryRepositoryBuilder = new ICategoryRepositoryBuilder();

        if (category is not null)
            categoryRepositoryBuilder.GetById(category);

        return new UpdateCategoryUseCase(
            categoryRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
