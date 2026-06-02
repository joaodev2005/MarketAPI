using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Order;
using MarketAPI.Domain.Security;

namespace MarketAPI.Application.UseCases.Order.List;

public class ListOrdersUseCase : IListOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggedUser _loggedUser;

    public ListOrdersUseCase(IOrderRepository orderRepository, ILoggedUser loggedUser)
    {
        _orderRepository = orderRepository;
        _loggedUser = loggedUser;
    }

    public async Task<List<ResponseOrderJson>> Execute()
    {
        var userId = _loggedUser.GetUserId();
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        return orders.Select(o => new ResponseOrderJson
        {
            Id = o.Id,
            Status = o.Status.ToString(),
            Total = o.Total,
            CreatedAt = o.CreatedAt,
            Items = o.Items.Select(i => new ResponseOrderItemJson
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Price,
                Subtotal = i.Price * i.Quantity
            }).ToList()
        }).ToList();
    }
}