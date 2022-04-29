using Inventory.Api.Base;
using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.Jwt;
using Inventory.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Api.Features.Util;

public class Api : ApiBase
{
    public const string BaseUrl = "util";

    public Api(IConvertorFactory convertorFactory, IMediator mediator) : base(convertorFactory, mediator)
    {
    }

    [FunctionName("test")]
    public async Task<string> Test([HttpTrigger(AuthorizationLevel.Function, "POST", Route = $"{BaseUrl}/Test")]string hello)
    {
        return await Task.FromResult(hello);
    }
}
