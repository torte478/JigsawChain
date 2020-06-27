namespace JigsawService.Extensions
{
    internal static class ParseExtensions
    {
        public static int ToInt(this string json)
        {
            return int.Parse(json);
        }
    }
}