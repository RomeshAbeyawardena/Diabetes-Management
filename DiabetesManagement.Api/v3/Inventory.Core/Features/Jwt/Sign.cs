﻿using Inventory.Contracts;
using Inventory.Features.Jwt;
using MediatR;

namespace Inventory.Core.Features.Jwt;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly IJwtProvider jwtProvider;

    public Sign(IJwtProvider jwtProvider)
    {
        this.jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtProvider.BuildToken(request.Dictionary!, request.Parameters ?? jwtProvider.DefaultTokenValidationParameters);
    }
}
