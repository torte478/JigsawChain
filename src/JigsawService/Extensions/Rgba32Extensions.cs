using System;
using SixLabors.ImageSharp.PixelFormats;

namespace JigsawService.Extensions
{
    internal static class Rgba32Extensions
    {
        public static Rgba32 ForEachComponent(this Rgba32 origin, Func<byte, byte> f)
        {
            return new Rgba32(f(origin.R), f(origin.G), f(origin.B));
        }
    }
}
