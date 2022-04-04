using DiabetesManagement.Shared.Contracts;
using System.Collections.Concurrent;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultDbModelCache : ConcurrentDictionary<Type, IDbModel>, IDbModelCache
    {
        private static Lazy<IDbModelCache> currentCache = new();
        public static IDbModelCache Current => currentCache.IsValueCreated 
            ? currentCache.Value 
            : (currentCache = new Lazy<IDbModelCache>(new DefaultDbModelCache())).Value;
    }
}
