using JigsawService.Extensions;
using JigsawService.Templets;

namespace JigsawService.Fake
{
    internal sealed class RawTemplets : IRawTemplets
    {
        public Maybe<Templet, string> Deserialize(string templet)
        {
            return new Templet(1, 2).Right<Templet, string>();
        }

        public string Serialize()
        {
            return "[{\"name\":\"public\",\"type\":\"bool\"}]";
        }
    }
}
