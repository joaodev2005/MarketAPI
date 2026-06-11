using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Order.List;
using MarketAPI.Domain.Entities;

namespace UseCases.Tests.Order.List;

public class ListOrdersUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var orders = new List<MarketAPI.Domain.Entities.Order>
    {
        new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Total = 100,
            Items =
            [
                new OrderItem
                {
                    Product = new MarketAPI.Domain.Entities.Product { Name = "Notebook" },
                    Quantity = 1,
                    Price = 100
                }
            ]
        }
    };

        var useCase = CreateUseCase(orders);

        var result = await useCase.Execute();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task Success_Empty_List()
    {
        var useCase = CreateUseCase([]);

        var result = await useCase.Execute();

        result.Should().BeEmpty();
    }

    private ListOrdersUseCase CreateUseCase(List<MarketAPI.Domain.Entities.Order> orders)
    {
        var orderRepositoryBuilder = new IOrderRepositoryBuilder();

        orderRepositoryBuilder.GetByUserId(orders);

        return new ListOrdersUseCase(
            orderRepositoryBuilder.Build(),
            new ILoggedUserBuilder().Build());
    }
}
