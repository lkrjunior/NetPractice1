using System;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies;
using ChallengeNet.Core.Interfaces;
using Xunit;

namespace ChallengeNet.Test.Handlers.TaxCalculateStrategy
{
    public class TaxCalculateHandlerTest
    {
        [Theory]
        [InlineData(typeof(TaxCalculateNfeStrategy), 3)]
        [InlineData(typeof(TaxCalculateNfceStrategy), 2)]
        public void ShouldTaxCalculateNfeAndNfceWhenValueIsValid(Type taxCalculateStrategy, double valueToValidate)
        {
            #region Arrange

            var value = 2;

            var expectedValue = value * valueToValidate;

            var taxCalcuateStrategy = (ITaxCalculateStrategy)Activator.CreateInstance(taxCalculateStrategy);

            var handler = new TaxCalculateHandler(taxCalcuateStrategy);

            #endregion

            #region Act

            var result = handler.CalculateTax(value);

            #endregion

            #region Assert

            Assert.Equal(expectedValue, result);

            #endregion
        }
    }
}
