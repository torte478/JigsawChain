using Microsoft.Extensions.Configuration;

namespace JigsawService.Extensions
{
    internal static class ConfigExtensions
    {
        public static Token Get(this IConfiguration config, string property)
        {
            return new Token(config, property);
        }

        public sealed class Token
        {
            private readonly IConfiguration config;
            private readonly string property;

            public Token(IConfiguration config, string property)
            {
                this.config = config;
                this.property = property;
            }

            public Token Get(string next)
            {
                return new Token(config, $"{property}:{next}");
            }

            public int ToInt()
            {
                return config[property]._(int.Parse);
            }
        }
    }
}