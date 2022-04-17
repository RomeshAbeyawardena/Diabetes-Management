using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;

namespace DiabetesManagement.Features;

public class AuthenticationRequestHandler<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : class
{
    private readonly IHttpContextAccessor httpContext;

    public AuthenticationRequestHandler(IHttpContextAccessor httpContext)
    {
        this.httpContext = httpContext;
    }

    public IHttpContextAccessor HttpContext => httpContext;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
