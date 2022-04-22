using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory;

public class UnauthorizedObjectResult : ObjectResult
{
    public UnauthorizedObjectResult(object result)
        : base(result)
    {
        StatusCode = StatusCodes.Status401Unauthorized;
    }

}
