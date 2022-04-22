﻿using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Application;

[RequiresClaims(Permissions.Application_Edit)]
public class PostCommand : IRequest<Models.Application>
{
    public string? AccessToken { get; set; }
    public string? Claims { get; set; }
    public string? DisplayName { get; set; }
    public TimeSpan? Expires { get; set; }
    public string? Intent { get; set; }
    public string? Name { get; set; }
    public Guid? UserId { get; set; }
}
