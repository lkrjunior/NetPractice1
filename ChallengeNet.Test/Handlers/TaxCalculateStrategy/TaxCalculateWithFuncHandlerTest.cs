using System;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using Moq;
using Xunit;

namespace ChallengeNet.Test.Handlers.TaxCalculateStrategy
{
    public class TaxCalculateWithFuncHandlerTest
    {
        private Func<ProductType, ITaxCalculateStrategy> InitializeFuncTaxCalculateStrategy()
        {
            return (ProductType productType) =>
            {
                return productType switch
                {
                    ProductType.Nfe => new TaxCalculateNfeStrategy(),
                    ProductType.Nfce => new TaxCalculateNfceStrategy(),
                    _ => throw new ArgumentException($"{nameof(ITaxCalculateStrategy)} not found"),
                };
            };
        }

        [Theory]
        [InlineData(ProductType.Nfe, 3)]
        [InlineData(ProductType.Nfce, 2)]
        public void ShouldTaxCalculateNfeAndNfceWithFuncWhenValueIsValid(ProductType productType, double valueToValidate)
        {
            #region Arrange

            var value = 2;

            var expectedValue = value * valueToValidate;

            Func<ProductType, ITaxCalculateStrategy> funcTaxCalculateStrategy = InitializeFuncTaxCalculateStrategy();

            var handler = new TaxCalculateWithFuncHandler(funcTaxCalculateStrategy);

            #endregion

            #region Act

            var result = handler.CalculateTax(productType, value);

            #endregion

            #region Assert

            Assert.Equal(expectedValue, result);

            #endregion
        }
    }
}
