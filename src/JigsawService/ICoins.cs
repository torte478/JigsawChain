using System.Threading.Tasks;
using JigsawService.Templets;

namespace JigsawService
{
    internal interface ICoins
    {
        Task<int> CalculateCostAsync(Templet templet);
    }
}