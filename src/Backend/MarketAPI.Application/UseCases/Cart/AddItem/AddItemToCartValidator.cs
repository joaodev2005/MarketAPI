using FluentValidation;
using MarketAPI.Communication.Requests;
using MarketAPI.Exception;

namespace MarketAPI.Application.UseCases.Cart.AddItem;

public class AddItemToCartValidator : AbstractValidator<RequestAddItemToCartJson>
{
    public AddItemToCartValidator()
    {
        RuleFor(r => r.ProductId)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PRODUCT_IS_REQUIRED);

        RuleFor(r => r.Quantity)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.QUANTITY_MUST_BE_GREATER_THAN_0);
    }
}