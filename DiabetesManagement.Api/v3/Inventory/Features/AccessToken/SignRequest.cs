﻿using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.AccessToken;
[RequiresClaims(Permissions.Anonymous_Access)]
public class SignRequest : IRequest<string>
{
    public bool Validate { get; set; }
    public string? ApiIntent { get; set; }
    public string? ApiKey { get; set; }
    public string? ApiChallenge { get; set; }
}