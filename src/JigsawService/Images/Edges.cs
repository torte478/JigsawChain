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

        public override string ToString()
        {
            return $"{ToChar(Top)}{ToChar(Right)}{ToChar(Bottom)}{ToChar(Left)}";
        }

        private static char ToChar(Edge edge)
        {
            return edge switch
            {
                Edge.Flat => 'f',
                Edge.Inside => 'i',
                _ => 'o'
            };
        }
    }
}
