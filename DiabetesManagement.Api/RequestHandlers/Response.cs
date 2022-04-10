namespace DiabetesManagement.Api.RequestHandlers
{
    public class Response
    {
        public Response(object data, int statusCode = 200, string statusMessage = default)
        {
            Data = data;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }

        public object Data { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }

    public class Response<T> : Response
    {
        public Response(T data, int statusCode = 200, string statusMessage = default)
            : base(data, statusCode, statusMessage)
        {
        }
    }
}
