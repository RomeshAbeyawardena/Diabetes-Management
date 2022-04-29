using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        public const string ValidateSessionKey = "validate-session";
        public ValidateSessionAttribute(bool validateSession = true)
        {
            Order = int.MinValue;
            ValidateSession = validateSession;
        }

        public bool ValidateSession { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var items = context.HttpContext.Items;
            if (!items.TryAdd(ValidateSessionKey, true))
            {
                items[ValidateSessionKey] = true;
            }
        }
    }
}
