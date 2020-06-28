using System;

namespace JigsawService.Extensions
{
    internal static class FunctionalExtensions
    {
        public static TOut _<TIn, TOut>(this TIn x, Func<TIn, TOut> f)
        {
            return f(x);
        }

        public static T _<T>(this T x, Action<T> p)
        {
            p(x);
            return x;   
        }
    }
}
