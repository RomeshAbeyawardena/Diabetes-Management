using System.ComponentModel.DataAnnotations;

namespace Inventory.Attributes;

public class RequiredIfAttribute : RequiredAttribute
{
    public RequiredIfAttribute()
    {

    }

    public override bool IsValid(object? value)
    {
        return base.IsValid(value);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var s = validationContext.Items;
        return base.IsValid(value, validationContext);
    }
}
