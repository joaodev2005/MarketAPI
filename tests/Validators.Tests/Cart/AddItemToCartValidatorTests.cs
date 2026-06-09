using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;
using MarketAPI.Application.UseCases.Cart.AddItem;
using MarketAPI.Exception;

namespace Validators.Tests.Cart;

public class AddItemToCartValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new AddItemToCartValidator();

        var request = RequestAddItemToCartJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_When_ProductId_Is_Empty()
    {
        var validator = new AddItemToCartValidator();

        var request = RequestAddItemToCartJsonBuilder.Build();
        request.ProductId = Guid.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.PRODUCT_IS_REQUIRED);
    }

    [Fact]
    public void Error_When_Quantity_Is_Zero()
    {
        var validator = new AddItemToCartValidator();

        var request = RequestAddItemToCartJsonBuilder.Build();
        request.Quantity = 0;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.QUANTITY_MUST_BE_GREATER_THAN_0);
    }

    [Fact]
    public void Error_When_Quantity_Is_Negative()
    {
        var validator = new AddItemToCartValidator();

        var request = RequestAddItemToCartJsonBuilder.Build();
        request.Quantity = -1;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.QUANTITY_MUST_BE_GREATER_THAN_0);
    }
}
