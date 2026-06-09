using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.User.Register;
using MarketAPI.Communication.Requests;

namespace Validators.Tests.User.Register;

public class RegisterUserAccountValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = "";

        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = "emailinvalido";

        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
