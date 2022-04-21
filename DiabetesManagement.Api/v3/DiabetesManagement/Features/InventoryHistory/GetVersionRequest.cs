﻿using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.InventoryHistory;
[RequiresClaims(Permissions.Inventory_View)]
public class GetVersionRequest : IRequest<int>
{
    public string? Subject { get; set; }
    public string? Intent { get; set; }
    public Guid? InventoryId { get; set; }
    public Guid? UserId { get; set; }
}
