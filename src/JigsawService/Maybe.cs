using System;
using JigsawService.Extensions;

namespace JigsawService
{
    internal struct Maybe<TRight, TLeft>
    {
        private readonly Lazy<TRight> right;
        private readonly Lazy<TLeft> left;

        public TRight Right { get { return right.Value; } }
        public bool IsRight { get; }
        public TLeft Left { get { return left.Value; } }
        public bool IsLeft { get { return !IsRight; } }

        public Maybe(bool isRight, Lazy<TRight> right, Lazy<TLeft> left) : this()
        {
            IsRight = isRight;
            this.right = right;
            this.left = left;
        }

        public static Maybe<TRight, TLeft> CreateRight(TRight value)
        {
            return new Maybe<TRight, TLeft>(
                        true,
                        new Lazy<TRight>(() => value),
                        new Lazy<TLeft>(() => throw new Exception("Maybe is right"))
                        );
        }

        public static Maybe<TRight, TLeft> CreateLeft(TLeft value)
        {
            return new Maybe<TRight, TLeft>(
                        false,
                        new Lazy<TRight>(() => throw new Exception("Maybe is left")),
                        new Lazy<TLeft>(() => value)
                        );
        }
    }
}