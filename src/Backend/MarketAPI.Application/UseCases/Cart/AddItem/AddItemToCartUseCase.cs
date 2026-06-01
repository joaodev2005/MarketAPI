using MarketAPI.Communication.Requests;
using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Cart;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Domain.Security;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Cart.AddItem;

public class AddItemToCartUseCase : IAddItemToCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToCartUseCase(
        ICartRepository cartRepository, 
        IProductRepository productRepository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestAddItemToCartJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var userId = _loggedUser.GetUserId();

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart is null)
        {
            cart = new Domain.Entities.Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };
            await _cartRepository.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem is not null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            var item = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            await _cartRepository.AddItemAsync(item);
        }

        await _unitOfWork.Commit();
    }
    
    private async Task ValidateAndThrowOnFailures(RequestAddItemToCartJson request)
    {
        var validator = new AddItemToCartValidator();
        var result = await validator.ValidateAsync(request);

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                string.Empty, "Product not found"));
        else if (product.Stock < request.Quantity)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                string.Empty, "Insufficient stock"));

        if (result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}