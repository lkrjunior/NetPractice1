using System;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies
{
    public class TaxCalculateNfceStrategy : ITaxCalculateStrategy
    {
        public ProductType ProductType => ProductType.Nfce;

        public double CalculateTax(double value)
        {
            return value * 2;
        }
    }
}
