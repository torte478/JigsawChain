using System.Threading.Tasks;
using JigsawService.Templets;

namespace JigsawService.Fake
{
    internal sealed class Coins : ICoins
    {
        public Task<int> CalculateCostAsync(Templet templet)
        {
            return Task.Run(() => 100);
        }
    }
}
