namespace DiabetesManagement.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetHash<T>(this T value)
        {
            return Convert.ToBase64String(MessagePack.MessagePackSerializer.Serialize(value));
        }
    }
}
