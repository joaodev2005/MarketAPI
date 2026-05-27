using System.Net;

namespace MarketAPI.Exception.ExceptionsBase;

public abstract class MarketAPIException : System.Exception
{
    public abstract HttpStatusCode GetStatusCode();
    public abstract List<string> GetErrorMessages();
}