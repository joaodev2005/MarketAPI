using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Enums;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Order;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Order.Admin.UpdateStatus;

public class UpdateOrderStatusUseCase : IUpdateOrderStatusUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderStatusUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestUpdateOrderStatusJson request)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            throw new NotFoundException("Order not found");

        order.Status = (OrderStatus)request.Status;

        await _unitOfWork.Commit();
    }
}