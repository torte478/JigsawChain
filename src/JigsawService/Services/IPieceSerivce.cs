using JigsawService.DB;
using System.Threading.Tasks;

namespace JigsawService.Services
{
    internal interface IPieceSerivce
    {
        Task<Maybe<string, string>> SaveTaskAsync(IRpcToken token, TaskInfo task);
    }
}