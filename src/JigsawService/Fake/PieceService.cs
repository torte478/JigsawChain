using System.Threading.Tasks;
using JigsawService.Extensions;

namespace JigsawService.Fake
{
    internal sealed class PieceService : IPieceSerivce
    {
        private readonly bool right;

        public PieceService(bool right)
        {
            this.right = right;
        }

        public Task<Maybe<string, string>> SaveTaskAsync(
                                                IRpcToken token, 
                                                TaskInfo task)
        {
            return Task.Run(() => right
                                  ? A<string, string>.Right("123345")
                                  : A<string, string>.Left("Can't register task"));
        }
    }
}
