using System;
using ChallengeNet.Core.Interfaces;

namespace ChallengeNet.Core.Handlers.TaxCalculateStrategy
{
    public class TaxCalculateHandler : ITaxCalculateHandler
    {
        private readonly ITaxCalculateStrategy _taxCalculateStrategy;

        public TaxCalculateHandler(ITaxCalculateStrategy taxCalculateStrategy)
        {
            _taxCalculateStrategy = taxCalculateStrategy;
        }

        public double CalculateTax(double value)
        {
            var result = _taxCalculateStrategy.CalculateTax(value);

            return result;
        }
    }
}