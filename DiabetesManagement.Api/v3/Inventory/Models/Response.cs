namespace DiabetesManagement.Models;

public class Response
{
    public Response(object? data, int statusCode, string statusMessage)
    {
        Data = data;
        StatusCode = statusCode;
        StatusMessage = statusMessage;
    }

    public Response(object? data)
        : this(data, 200, string.Empty)
    {
        
    }

    public Response(int statusCode, string statusMessage)
        : this(null!, statusCode, statusMessage)
    {

    }

    public int StatusCode { get; }
    public string StatusMessage { get; }
    public object? Data { get; }
}
