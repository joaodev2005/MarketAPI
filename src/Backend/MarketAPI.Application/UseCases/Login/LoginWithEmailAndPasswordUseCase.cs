using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Security.PasswordHashing;
using MarketAPI.Domain.Security.Tokens;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Login;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public LoginWithEmailAndPasswordUseCase(
        IPasswordHasher passwordHasher, 
        IUserReadOnlyRepository userReadOnlyRepository, 
        IAccessTokenGenerator tokenGenerator)
    {
        _passwordHasher = passwordHasher;
        _userReadOnlyRepository = userReadOnlyRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);
        if (user is null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
        if (!isPasswordValid)
            throw new InvalidLoginException();

        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _tokenGenerator.Generate(user)
            }
        };
    }
}