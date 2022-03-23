namespace DiabetesManagement.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlerDescriptorAttribute : Attribute
    {
        public HandlerDescriptorAttribute(string queryOrCommand)
        {
            QueryOrCommand = queryOrCommand;
        }

        public string QueryOrCommand { get; }
    }
}
