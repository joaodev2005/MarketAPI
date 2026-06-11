using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Order.Admin.UpdateStatus;
using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Enums;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Order.Admin.UpdateStatus;

public class UpdateOrderStatusUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var order = new MarketAPI.Domain.Entities.Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Pending
        };

        var request = new RequestUpdateOrderStatusJson
        {
            Status = (int)OrderStatus.Paid
        };

        var useCase = CreateUseCase(order);

        await useCase.Execute(order.Id, request);

        order.Status.Should().Be(OrderStatus.Paid);
    }

    [Fact]
    public async Task Error_Order_Not_Found()
    {
        var request = new RequestUpdateOrderStatusJson
        {
            Status = (int)OrderStatus.Paid
        };

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private UpdateOrderStatusUseCase CreateUseCase(MarketAPI.Domain.Entities.Order? order = null)
    {
        var orderRepositoryBuilder = new IOrderRepositoryBuilder();

        if (order is not null)
            orderRepositoryBuilder.GetById(order);

        return new UpdateOrderStatusUseCase(
            orderRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
