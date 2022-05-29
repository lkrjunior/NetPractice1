using System;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Handlers.TaxCalculateStrategy
{
    public class TaxCalculateWithFuncHandler : ITaxCalculateWithFuncHandler
    {
        private readonly Func<ProductType, ITaxCalculateStrategy> _funcTaxCalculateStrategy;

        public TaxCalculateWithFuncHandler(Func<ProductType, ITaxCalculateStrategy> funcTaxCalculateStrategy)
        {
            _funcTaxCalculateStrategy = funcTaxCalculateStrategy;
        }

        public double CalculateTax(ProductType productType, double value)
        {
            var result = _funcTaxCalculateStrategy(productType).CalculateTax(value);

            return result;
        }
    }
}
