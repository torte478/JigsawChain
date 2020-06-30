using System.Threading.Tasks;

namespace JigsawService
{
    internal interface IPieceSerivce
    {
        Task<Maybe<string, string>> SaveTaskAsync(IRpcToken token, TaskInfo task);
    }
}