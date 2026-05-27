using System.Net;

namespace MarketAPI.Exception.ExceptionsBase;

public class InvalidLoginException : MarketAPIException
{
    public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}