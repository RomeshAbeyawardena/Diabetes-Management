using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IRequestHandler
    {
        Task Handle(object request);
    }

    public interface IRequestHandler<TRequest> : IRequestHandler
    {
        Task Handle(TRequest request);
    }

    public interface IRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest>
    {
        new Task<TResponse> Handle(TRequest request);
    }
}
