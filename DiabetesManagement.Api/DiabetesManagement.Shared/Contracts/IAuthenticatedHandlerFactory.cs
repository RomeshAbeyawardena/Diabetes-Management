﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IAuthenticatedHandlerFactory : IHandlerFactory
    {
        Task<bool> IsAuthenticated(string key, string secret);
    }
}