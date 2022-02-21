using System.Collections.Generic;

namespace FlagshipProductTest.Shared.Services
{
    public class MessagePack
    {
        public MessagePack(string defaultMessage)
        {
            DefaultMessage = defaultMessage;
        }

        public string DefaultMessage { get; set; }
        public IDictionary<string, string> Mappings { get; set; }
    }
}
