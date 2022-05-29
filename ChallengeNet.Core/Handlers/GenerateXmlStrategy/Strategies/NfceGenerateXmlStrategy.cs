using System;
using System.Xml.Linq;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.AttributeStrategy;

namespace ChallengeNet.Core.Handlers.GenerateXmlStrategy.Strategies
{
    [ProductTypeStrategyAttribute(ProductType.Nfce)]
    public class NfceGenerateXmlStrategy : GenerateXmlStrategyBase
    {
        protected override XElement GetElementFromProduct()
        {
            var element = new XElement(nameof(ProductType.Nfce));

            return element;
        }
    }
}