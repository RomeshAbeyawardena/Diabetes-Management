using Inventory.Attributes;
using Inventory.Contracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Inventory.Core.Defaults
{
    [RegisterService]
    public class DefaultConvertorFactory : IConvertorFactory
    {
        private readonly IEnumerable<IConvertor> convertors;
        private readonly ILogger<IConvertorFactory> logger;

        public DefaultConvertorFactory(ILogger<IConvertorFactory> logger, IEnumerable<IConvertor> convertors)
        {
            this.convertors = convertors.OrderBy(c => c.OrderIndex);
            this.logger = logger;
        }

        public IConvertor? GetConvertor(JsonElement element)
        {
            var convertor = convertors.FirstOrDefault(c => c.CanConvert(element));
            var convertorType = convertor.GetType();
            var rawValue = element.GetRawText();
            logger.LogInformation("Using {convertorType}({OrderIndex}) for {rawValue}", convertor.OrderIndex, convertorType, rawValue);
            return convertor;
        }
    }
}
