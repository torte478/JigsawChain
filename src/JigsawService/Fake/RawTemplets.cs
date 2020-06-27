namespace JigsawService.Fake
{
    internal sealed class RawTemplets : IRawTemplets
    {
        public string Serialize()
        {
            return "[{\"name\":\"public\",\"type\":\"bool\"}]";
        }
    }
}
