using System;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies
{
    public class TaxCalculateNfeStrategy : ITaxCalculateStrategy
    {
        public ProductType ProductType => ProductType.Nfe;

        public double CalculateTax(double value)
        {
            return value * 3;
        }
    }
}
