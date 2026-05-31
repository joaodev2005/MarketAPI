using System.Net;

namespace MarketAPI.Exception.ExceptionsBase;

public class ErrorOnValidationException : MarketApiException
{
    private readonly List<string> _errors;

    public ErrorOnValidationException(List<string> errorMessages)
    {
        _errors = errorMessages;
    }

    public override List<string> GetErrorMessages() => _errors;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}