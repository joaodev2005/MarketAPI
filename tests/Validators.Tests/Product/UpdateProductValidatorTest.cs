using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.Create;
using MarketAPI.Application.UseCases.Product.Update;
using MarketAPI.Exception;

namespace Validators.Tests.Product;

public class UpdateProductValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestProductJsonBuilder.Build();

        var validator = new UpdateProductValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_When_Name_Is_Empty()
    {
        var validator = new UpdateProductValidator();

        var request = RequestProductJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }

    [Fact]
    public void Error_When_Price_Is_Zero()
    {
        var validator = new UpdateProductValidator();

        var request = RequestProductJsonBuilder.Build();
        request.Price = 0;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.PRICE_MUST_BE_GREATHER_THAN_0);
    }

    [Fact]
    public void Error_When_Stock_Is_Negative()
    {
        var validator = new UpdateProductValidator();

        var request = RequestProductJsonBuilder.Build();
        request.Stock = -1;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.STOCK_CANNOT_BE_NEGATIVE);
    }

    [Fact]
    public void Error_When_CategoryId_Is_Empty()
    {
        var validator = new UpdateProductValidator();

        var request = RequestProductJsonBuilder.Build();
        request.CategoryId = Guid.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.CATEGORY_IS_REQUIRED);
    }
}
