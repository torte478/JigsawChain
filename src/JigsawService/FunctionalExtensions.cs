﻿using System;

namespace JigsawService
{
    internal static class FunctionalExtensions
    {
        public static TOut _<TIn, TOut>(this TIn x, Func<TIn, TOut> f)
        {
            return f(x);
        }
    }
}
