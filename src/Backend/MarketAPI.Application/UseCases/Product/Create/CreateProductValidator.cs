using FluentValidation;
using MarketAPI.Communication.Requests;
using MarketAPI.Exception;

namespace MarketAPI.Application.UseCases.Product.Create;

public class CreateProductValidator : AbstractValidator<RequestProductJson>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.PRICE_MUST_BE_GREATHER_THAN_0);

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.STOCK_CANNOT_BE_NEGATIVE);

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.CATEGORY_IS_REQUIRED);
    }
}