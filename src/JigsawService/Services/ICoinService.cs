﻿using System.Threading.Tasks;
using JigsawService.Templets;

namespace JigsawService.Services
{
    internal interface ICoinService
    {
        Task<int> CalculateCostAsync(Templet templet);
        Task<Maybe<bool, string>> TryPayJigsawCreationAsync(
                                                        IRpcToken token,
                                                        int cost);
    }
}