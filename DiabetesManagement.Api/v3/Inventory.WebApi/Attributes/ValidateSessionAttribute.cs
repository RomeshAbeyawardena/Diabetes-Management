namespace Inventory.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateSessionAttribute : Attribute
    {
        public ValidateSessionAttribute(bool validateSession = true)
        {
            ValidateSession = validateSession;
        }

        public bool ValidateSession { get; }
    }
}
