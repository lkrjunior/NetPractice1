using System;
using System.Xml.Linq;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.AttributeStrategy;

namespace ChallengeNet.Core.Handlers.GenerateXmlStrategy.Strategies
{
    [ProductTypeStrategyAttribute(ProductType.Nfe)]
    public class NfeGenerateXmlStrategy : GenerateXmlStrategyBase
    {
        protected override XElement GetElementFromProduct()
        {
            var element = new XElement(nameof(ProductType.Nfe));

            return element;
        }
    }
}