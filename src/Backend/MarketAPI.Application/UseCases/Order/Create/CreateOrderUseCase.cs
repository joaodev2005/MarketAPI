using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Enums;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Cart;
using MarketAPI.Domain.Repositories.Order;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Domain.Security;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Order.Create;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderUseCase(
        ICartRepository cartRepository, 
        IOrderRepository orderRepository, 
        IProductRepository productRepository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseOrderJson> Execute()
    {
        var userId = _loggedUser.GetUserId();
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart is null || !cart.Items.Any())
            throw new ErrorOnValidationException(["Cart is empty"]);

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product!.Stock < item.Quantity)
                throw new ErrorOnValidationException([$"Insufficient stock for {product.Name}"]);
        }

        var productNames = cart.Items.ToDictionary(
            i => i.ProductId,
            i => i.Product.Name);

        var order = new Domain.Entities.Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Items = cart.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Product.Price
            }).ToList()
        };

        order.Total = order.Items.Sum(i => i.Price * i.Quantity);

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            product!.Stock -= item.Quantity;
        }

        await _orderRepository.AddAsync(order);
        await _cartRepository.ClearAsync(cart);
        await _unitOfWork.Commit();

        return new ResponseOrderJson
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new ResponseOrderItemJson
            {
                ProductId = i.ProductId,
                ProductName = productNames[i.ProductId],
                Quantity = i.Quantity,
                Price = i.Price,
                Subtotal = i.Price * i.Quantity
            }).ToList()
        };
    }
}