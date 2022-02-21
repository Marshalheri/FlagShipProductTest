using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlagshipProductTest.Shared.Services
{
    public class MessagePackProvider : IMessagePackProvider
    {
        private readonly MessagePackSettings _settings;
        private readonly IDictionary<string, MessagePack> _packs;
        private readonly ILogger _logger;
        public MessagePackProvider(IOptions<MessagePackSettings> settingsProvider,
            ILogger<MessagePackProvider> logger)
        {
            _settings = settingsProvider.Value;
            _logger = logger;
            _packs = new Dictionary<string, MessagePack>();
            CreateLanguagePacks();
        }

        private void CreateLanguagePacks()
        {
            var serializer = JsonSerializer.CreateDefault();

            try
            {
                var pack = new MessagePack(_settings.DefaultMessage);
                if (!File.Exists(_settings.BaseLocation))
                {
                    _logger.LogWarning("No file not found.");
                    return;
                }
                var reader = File.OpenText(_settings.BaseLocation);

                pack.Mappings = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
                _packs.Add("default", pack);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load language pack.");
            }
        }

        public MessagePack GetPack()
        {
            return (_packs.TryGetValue("default", out var pack)) ? pack : null;
        }
    }
}
