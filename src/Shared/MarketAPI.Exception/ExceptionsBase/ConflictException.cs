using System.Net;

namespace MarketAPI.Exception.ExceptionsBase;

public class ConflictException : MarketApiException
{
    private readonly string _message;

    public ConflictException(string message)
    {
        _message = message;
    }

    public override List<string> GetErrorMessages() => [_message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Conflict;
}