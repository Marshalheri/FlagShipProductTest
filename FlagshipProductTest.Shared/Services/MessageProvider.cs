using System;

namespace FlagshipProductTest.Shared.Services
{
    public class MessageProvider : IMessageProvider
    {
        readonly IMessagePackProvider _provider;
        public MessageProvider(IMessagePackProvider provider)
        {
            _provider = provider;
        }

        public string GetMessage(string code)
        {
            var bundle = _provider.GetPack();
            if (bundle == null)
            {
                throw new Exception("Invalid language configuration");
            }
            if (bundle.Mappings.TryGetValue(code, out var message))
            {
                return message;
            }
            return bundle.DefaultMessage;
        }
    }
}
