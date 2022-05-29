using System;
using System.Xml.Linq;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Handlers.GenerateXmlStrategy.Strategies
{
    public abstract class GenerateXmlStrategyBase : IGenerateXmlStrategy
    {
        protected abstract XElement GetElementFromProduct();

        public XDocument GenerateXml(ProductType productType)
        {
            var elementBase = new XElement("XmlGenerator");

            var element = GetElementFromProduct();

            elementBase.Add(element);

            var xmlDocument = new XDocument(elementBase);

            return xmlDocument;
        }
    }
}