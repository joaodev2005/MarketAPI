using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Order;
using MarketAPI.Domain.Security;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Order.GetById;

public class GetOrderByIdUseCase : IGetOrderByIdUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggedUser _loggedUser;

    public GetOrderByIdUseCase(IOrderRepository orderRepository, ILoggedUser loggedUser)
    {
        _orderRepository = orderRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseOrderJson> Execute(Guid id)
    {var userId = _loggedUser.GetUserId();
        var order = await _orderRepository.GetByIdAsync(id);

        if (order is null || order.UserId != userId)
            throw new NotFoundException("Order not found");

        return new ResponseOrderJson
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new ResponseOrderItemJson
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Price,
                Subtotal = i.Price * i.Quantity
            }).ToList()
        };
    }
}