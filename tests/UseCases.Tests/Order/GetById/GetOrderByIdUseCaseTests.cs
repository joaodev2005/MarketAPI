using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Order.GetById;
using MarketAPI.Domain.Entities;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Order.GetById;

public class GetOrderByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var userId = Guid.NewGuid();

        var order = new MarketAPI.Domain.Entities.Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Total = 2000,
            Items =
            [
                new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    Price = 1000,
                    Product = new MarketAPI.Domain.Entities.Product
                    {
                        Name = "Notebook"
                    }
                }
            ]
        };

        var useCase = CreateUseCase(order, userId);

        var result = await useCase.Execute(order.Id);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Total.Should().Be(2000);
        result.Items.First().ProductName.Should().Be("Notebook");
    }

    [Fact]
    public async Task Error_Order_Not_Found()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Error_Order_Belongs_To_Another_User()
    {
        var order = new MarketAPI.Domain.Entities.Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(), 
            Total = 2000
        };

        var loggedUserId = Guid.NewGuid(); 

        var useCase = CreateUseCase(order, loggedUserId);

        var act = async () => await useCase.Execute(order.Id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private GetOrderByIdUseCase CreateUseCase(
        MarketAPI.Domain.Entities.Order? order = null,
        Guid? loggedUserId = null)
    {
        var orderRepositoryBuilder = new IOrderRepositoryBuilder();
        var loggedUserBuilder = new ILoggedUserBuilder();

        if (order is not null)
            orderRepositoryBuilder.GetById(order);

        if (loggedUserId.HasValue)
            loggedUserBuilder.GetUserId(loggedUserId.Value);

        return new GetOrderByIdUseCase(
            orderRepositoryBuilder.Build(),
            loggedUserBuilder.Build());
    }
}
