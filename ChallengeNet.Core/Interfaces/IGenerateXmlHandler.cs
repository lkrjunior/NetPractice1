using System;
using System.Xml.Linq;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Interfaces
{
    public interface IGenerateXmlHandler
    {
        XDocument GenerateXml(ProductType productType);
    }
}