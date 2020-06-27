using JigsawService.Extensions;
using JigsawService.Templets;

namespace JigsawService.Fake
{
    internal sealed class RawTemplets : IRawTemplets
    {
        public Maybe<Templet, string> Deserialize(string templet)
        {
            return A<Templet, string>.Right(new Templet(1, 2));
        }

        public string Serialize()
        {
            return "[{\"name\":\"public\",\"type\":\"bool\"}]";
        }
    }
}
