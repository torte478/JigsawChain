using System;
using System.Runtime.CompilerServices;

namespace JigsawService.Extensions
{
    internal static class MaybeExtensions
    {
        public static Maybe<TRight, TLeft> Right<TRight, TLeft>(this TRight value)
        {
            return Maybe<TRight, TLeft>.CreateRight(value);
        }

        public static Maybe<TRight, TLeft> Left<TRight, TLeft>(this TLeft value)
        {
            return Maybe<TRight, TLeft>.CreateLeft(value);
        }

        public static void Match<TRight, TLeft>(
                            this Maybe<TRight, TLeft> origin, 
                            Action<TRight> right, 
                            Action<TLeft> left)
        {
            if (origin.IsRight)
                right(origin.Right);
            else
                left(origin.Left);
        }

        public static Maybe<TRightOut, TLeftOut> Match
                        <TRightIn, TRightOut, TLeftIn, TLeftOut>(
                                            this Maybe<TRightIn, TLeftIn> origin,
                                            Func<TRightIn, TRightOut> right,
                                            Func<TLeftIn, TLeftOut> left)
        {
            return origin.IsRight
                   ? Right<TRightOut, TLeftOut>(right(origin.Right))
                   : Left<TRightOut, TLeftOut>(left(origin.Left));
        }

        public static Maybe<TRightOut, TLeft> Monad<TRightIn, TRightOut, TLeft>(
                            this Maybe<TRightIn, TLeft> origin,
                            Func<TRightIn, TRightOut> f)
        {
            return origin.IsRight
                   ? Right<TRightOut, TLeft>(f(origin.Right))
                   : Left<TRightOut, TLeft>(origin.Left);
        }

        public static Maybe<TRight, TLeft> MonadUp<TRight, TLeft>(
                                    this Maybe<Maybe<TRight, TLeft>, TLeft> origin)
        {
            return origin.IsRight
                   ? origin.Right
                   : Left<TRight, TLeft>(origin.Left);
        }

        public static Maybe<TRightOut, TLeft> IfRight<TRightIn, TRightOut, TLeft>(
                                    this Maybe<TRightIn, TLeft> origin,
                                    Func<TRightIn, Maybe<TRightOut, TLeft>> f)
        {
            return origin.IsRight
                   ? f(origin.Right)
                   : Left<TRightOut, TLeft>(origin.Left);
        }
    }
}