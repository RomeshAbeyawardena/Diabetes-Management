using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        public const string ValidateSessionKey = "validate-session";
        public const string UserIdValueKey = "user-id";
        public ValidateSessionAttribute(string userIdKey = "userId", bool validateSession = true)
        {
            Order = int.MinValue;
            UserIdKey = userIdKey;
            ValidateSession = validateSession;
        }

        public string UserIdKey { get; }
        public bool ValidateSession { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var items = context.HttpContext.Items;
            if (!items.TryAdd(ValidateSessionKey, true))
            {
                items[ValidateSessionKey] = true;
            }

            if(context.ModelState.TryGetValue(UserIdKey, out var entry) 
                && Guid.TryParse(entry.RawValue?.ToString(), out var userId))
            {
                if (!items.TryAdd(UserIdValueKey, userId))
                {
                    items[UserIdValueKey] = userId;
                }
            }
        }
    }
}
