namespace DiabetesManagement.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Copy<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> values)
        {
            foreach(var (key, value) in values)
            {
                if(!dictionary.TryAdd(key, value))
                {
                    dictionary[key] = value;
                }
            }
        }
    }
}
