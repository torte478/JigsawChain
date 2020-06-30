using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using JigsawService.Extensions;
using Newtonsoft.Json;

namespace JigsawService.Templets
{
    internal sealed class RawTemplets : IRawTemplets
    {
        private readonly ILogger<RawTemplets> logger;

        public RawTemplets(ILogger<RawTemplets> logger)
        {
            this.logger = logger;
        }

        public Maybe<Templet, string> Deserialize(string templet)
        {
            try
            {
                return templet
                       ._(JObject.Parse)
                       ._(_ => new Templet(
                           _["tolerancy"].Value<int>(),
                           _["noise"].Value<int>()))
                       .Right<Templet, string>();

            }
            catch (Exception ex)
            {
                var message = $"Parsing templet error: {ex.Message}";
                logger.LogInformation(message);
                return message.Left<Templet, string>();
            }
        }

        public string Serialize()
        {
            return new JArray
            {
                new JObject
                {
                    { "name", "tolerancy" },
                    { "type", "int" },
                    { "min", 0 },
                    { "max", 100 }
                },
                new JObject
                {
                    { "name", "noise" },
                    { "type", "int" },
                    { "min", 0 },
                    { "max", 100 }
                },
            }
            ._(JsonConvert.SerializeObject);
        }
    }
}