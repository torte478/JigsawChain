using System.Threading.Tasks;
using JigsawService.DB;
using JigsawService.Extensions;
using JigsawService.Services;

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
                                  ? "123345".Right<string, string>()
                                  : "Can't register task".Left<string, string>());
        }
    }
}
