using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.PasswordHashing;
using CommonTestUtilities.Security.Tokens;
using MarketAPI.Application.UseCases.Login;
using MarketAPI.Domain.Extensions;
using Shouldly;

namespace UseCases.Tests.Login.LoginWithEmailAndPassword;

public class LoginWithEmailAndPasswordTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestLoginJsonBuilder.Build();

        var user = new MarketAPI.Domain.Entities.User
        {
            Name = "João",
            Email = request.Email,
            Password = "hashed-password"
        };

        var useCase = CreateUseCase(
            password: request.Password,
            user: user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldBe("fake-access-token");
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, MarketAPI.Domain.Entities.User? user = null)
    {
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        var tokenGenerator = new AccessTokenGeneratorBuilder().Build();
        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        if (password.IsNotEmpty())
            passwordHasherBuilder.VerifyPassword(password);

        return new LoginWithEmailAndPasswordUseCase(
            passwordHasherBuilder.Build(),
            userReadOnlyRepositoryBuilder.Build(),
            tokenGenerator
        );
    }
}
