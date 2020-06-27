namespace JigsawService.Extensions
{
    internal static class A<TRight, TLeft>
    {
        public static Maybe<TRight, TLeft> Right(TRight value)
        {
            return Maybe<TRight, TLeft>.CreateRight(value);
        }

        public static Maybe<TRight, TLeft> Left(TLeft value)
        {
            return Maybe<TRight, TLeft>.CreateLeft(value);
        }
    }
}