using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Api
{
    public class UnauthorisedObjectResult : ObjectResult
    {
        public UnauthorisedObjectResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
