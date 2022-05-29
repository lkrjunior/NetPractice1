using System;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Interfaces
{
    public interface ITaxCalculateWithFuncHandler
    {
        double CalculateTax(ProductType productType, double value);
    }
}
