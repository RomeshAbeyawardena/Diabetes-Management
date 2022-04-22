using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;

namespace Inventory.Contracts
{
    public interface IAuthenticationRequestHandler<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        IHttpContextAccessor HttpContext { get; }
    }
}
