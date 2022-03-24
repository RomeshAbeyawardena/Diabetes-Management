namespace DiabetesManagement.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlerDescriptorAttribute : Attribute
    {
        public HandlerDescriptorAttribute(string queryOrCommand)
        {
            QueryOrCommand = queryOrCommand;
        }

        public HandlerDescriptorAttribute(string queryOrCommand, params string[] requiredPermissions)
            : this(queryOrCommand)
        {
            RequiredPermissions = requiredPermissions;
        }

        public string QueryOrCommand { get; }
        public IEnumerable<string> RequiredPermissions { get; } = Array.Empty<string>();
    }
}
