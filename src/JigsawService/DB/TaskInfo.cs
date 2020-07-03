using JigsawService.Images;
using JigsawService.Templets;

namespace JigsawService.DB
{
    internal struct TaskInfo
    {
        public string ImageId { get; set; }
        public Templet Templet { get; set; }
        public Edges[,] Edges { get; set; }
        public int Cost { get; set; }

    }
}