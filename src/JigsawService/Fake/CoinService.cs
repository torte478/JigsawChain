using System.Threading.Tasks;
using JigsawService.Extensions;
using JigsawService.Services;
using JigsawService.Templets;

namespace JigsawService.Fake
{
    internal sealed class CoinService : ICoinService
    {
        private readonly bool right;

        public CoinService(bool right)
        {
            this.right = right;
        }

        public Task<int> CalculateCostAsync(Templet templet)
        {
            return Task.Run(() => 100);
        }

        public Task<Maybe<bool, string>> TryPayJigsawCreationAsync(
                                                    IRpcToken token, 
                                                    int cost)
        {
            return Task.Run(() => right
                                  ? true.Right<bool, string>()
                                  : "Can't pay operation".Left<bool, string>());
        }
    }
}
