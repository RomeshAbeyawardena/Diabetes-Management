using Inventory.Attributes;
using Inventory.Contracts;

namespace Inventory.Core.Defaults
{
    [RegisterService]
    public class DefaultConvertorFactory : IConvertorFactory
    {
        private readonly IEnumerable<IConvertor> convertors;

        public DefaultConvertorFactory(IEnumerable<IConvertor> convertors)
        {
            this.convertors = convertors;
        }

        public IConvertor? GetConvertor(Type type, object value)
        {
            return convertors.SingleOrDefault(c => c.CanConvert(type, value));
        }

        public IConvertor? GetConvertor<T>(T value)
        {
            return GetConvertor(typeof(T), value!);
        }
    }
}
