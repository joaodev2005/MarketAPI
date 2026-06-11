using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Order.Admin.GetAll;
using MarketAPI.Domain.Entities;

namespace UseCases.Tests.Order.Admin.GetAll;

public class GetAllOrdersUseCaseTests
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
                        Product = new MarketAPI.Domain.Entities.Product
                        {
                            Name = "Notebook"
                        },
                        Quantity = 1,
                        Price = 100
                    }
                ]
            }
        };

        var useCase = CreateUseCase(orders);

        var result = await useCase.Execute();

        result.Should().HaveCount(1);
        result.First().Total.Should().Be(100);
        result.First().Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task Success_Empty_List()
    {
        var useCase = CreateUseCase([]);

        var result = await useCase.Execute();

        result.Should().BeEmpty();
    }

    private GetAllOrdersUseCase CreateUseCase(List<MarketAPI.Domain.Entities.Order> orders)
    {
        var orderRepositoryBuilder = new IOrderRepositoryBuilder();

        orderRepositoryBuilder.GetAll(orders);

        return new GetAllOrdersUseCase(
            orderRepositoryBuilder.Build());
    }
}
