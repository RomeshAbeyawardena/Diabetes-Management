using Inventory.Attributes;
using Inventory.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Inventory.Core.Defaults
{
    [RegisterService(ServiceLifetime.Transient)]
    public class DefaultConvertorFactory : IConvertorFactory
    {
        private readonly IEnumerable<IConvertor> convertors;
        
        public DefaultConvertorFactory(IEnumerable<IConvertor> convertors)
        {
            this.convertors = convertors.OrderBy(c => c.OrderIndex);
        }

        public IConvertor? GetConvertor(JsonElement element)
        {
            return convertors.FirstOrDefault(c => c.CanConvert(element));
        }
    }
}
