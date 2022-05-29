using System;
using ChallengeNet.Core.Handlers.GenerateXmlStrategy;
using ChallengeNet.Core.Models;
using Xunit;

namespace ChallengeNet.Test.Handlers.GenerateXmlStrategy
{
    public class GenerateXmlHandlerTest
    {
        [Fact]
        public void ShouldGenerateXmlNfeWhenInvokeMethod()
        {
            #region Arrange

            var expectedElement = nameof(ProductType.Nfe);

            var handler = new GenerateXmlHandler();

            #endregion

            #region Act

            var result = handler.GenerateXml(ProductType.Nfe);

            #endregion

            #region Assert

            var element = result.Root.Element(nameof(ProductType.Nfe)).Name;

            Assert.Equal(expectedElement, element);

            #endregion
        }

        [Fact]
        public void ShouldGenerateXmlNfceWhenInvokeMethod()
        {
            #region Arrange

            var expectedElement = nameof(ProductType.Nfce);

            var handler = new GenerateXmlHandler();

            #endregion

            #region Act

            var result = handler.GenerateXml(ProductType.Nfce);

            #endregion

            #region Assert

            var element = result.Root.Element(nameof(ProductType.Nfce)).Name;

            Assert.Equal(expectedElement, element);

            #endregion
        }
    }
}
