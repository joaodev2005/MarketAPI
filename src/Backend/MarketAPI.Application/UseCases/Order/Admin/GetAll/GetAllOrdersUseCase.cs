using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Order;

namespace MarketAPI.Application.UseCases.Order.Admin.GetAll;

public class GetAllOrdersUseCase : IGetAllOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;
    
    public GetAllOrdersUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<List<ResponseOrderJson>> Execute()
    {
        var orders = await _orderRepository.GetAllAsync();

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