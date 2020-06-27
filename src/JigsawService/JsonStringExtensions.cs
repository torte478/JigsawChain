namespace JigsawService.Extensions
{
    internal static class JsonStringExtensions
    {
        public static int ToInt(this string json)
        {
            return int.Parse(json);
        }
    }
}