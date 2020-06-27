namespace JigsawService.Templets
{
    internal struct Templet
    {
        public int Tolerancy { get; }
        public int Noise { get; }

        public Templet(int tolerany, int noise)
        {
            Tolerancy = tolerany;
            Noise = noise;
        }
    }
}