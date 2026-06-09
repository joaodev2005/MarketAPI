using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Category.Create;
using MarketAPI.Exception;

namespace Validators.Tests.Category;

public class CreateCategoryValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new CreateCategoryValidator();

        var request = RequestCategoryJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_When_Name_Is_Empty()
    {
        var validator = new CreateCategoryValidator();

        var request = RequestCategoryJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(error =>
            error.ErrorMessage == ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }
}
