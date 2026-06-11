using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories.Order;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IOrderRepositoryBuilder
{
    private readonly Mock<IOrderRepository> _mock = new();

    public void Add(Order order)
    {
        _mock.Setup(x => x.AddAsync(It.IsAny<Order>()))
             .Returns(Task.CompletedTask);
    }

    public IOrderRepositoryBuilder GetById(Order order)
    {
        _mock.Setup(x => x.GetByIdAsync(order.Id))
             .ReturnsAsync(order);

        return this;
    }

    public IOrderRepositoryBuilder GetByUserId(List<Order> orders)
    {
        _mock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync(orders);

        return this;
    }

    public IOrderRepositoryBuilder GetAll(List<Order> orders)
    {
        _mock.Setup(x => x.GetAllAsync())
             .ReturnsAsync(orders);

        return this;
    }

    public IOrderRepository Build() => _mock.Object;
}
