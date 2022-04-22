using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiabetesManagement;

public class UnauthorizedObjectResult : ObjectResult
{
    public UnauthorizedObjectResult(object result)
        : base(result)
    {
        StatusCode = StatusCodes.Status401Unauthorized;
    }
    
}
