using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Category.Create;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Category.Create;

public class CreateCategoryUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCategoryJsonBuilder.Build();

        var useCase = CreateUseCase();
        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_When_Name_Is_Empty()
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = "";

        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_When_Name_Already_Exists()
    {
        var request = RequestCategoryJsonBuilder.Build();

        var useCase = CreateUseCase(nameExists: true);
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    private CreateCategoryUseCase CreateUseCase(bool nameExists = false)
    {
        var categoryRepositoryBuilder = new ICategoryRepositoryBuilder();

        if (nameExists)
            categoryRepositoryBuilder.ExistsByName(true);

        return new CreateCategoryUseCase(
            categoryRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
