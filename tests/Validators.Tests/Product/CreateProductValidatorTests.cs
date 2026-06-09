using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.Create;

namespace Validators.Tests.Product;

public class CreateProductValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestProductJsonBuilder.Build();

        var validator = new CreateProductValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestProductJsonBuilder.Build();
        request.Name = "";

        var validator = new CreateProductValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Price_Zero()
    {
        var request = RequestProductJsonBuilder.Build();
        request.Price = 0;

        var validator = new CreateProductValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Stock_Negative()
    {
        var request = RequestProductJsonBuilder.Build();
        request.Stock = -1;

        var validator = new CreateProductValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }
}
