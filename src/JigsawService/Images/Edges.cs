namespace JigsawService.Images
{
    internal struct Edges
    {
        public Edge Top { get; }
        public Edge Right { get; }
        public Edge Bottom { get; }
        public Edge Left { get; }

        public Edges(Edge top, Edge right, Edge bottom, Edge left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}
