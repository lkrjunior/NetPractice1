using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.AttributeStrategy;

namespace ChallengeNet.Core.Handlers.GenerateXmlStrategy
{
    public class GenerateXmlHandler : IGenerateXmlHandler
    {
        private static IDictionary<ProductType, Type> InitializeStrategiesMap()
        {
            var _strategyeMaps = new Dictionary<ProductType, Type>();

            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var item in types.Where(x => (typeof(IGenerateXmlStrategy)).IsAssignableFrom(x) && !x.IsInterface))
            {
                var strategies = item.GetCustomAttributes<ProductTypeStrategyAttribute>();

                foreach (var strategyId in strategies)
                {
                    if (strategyId != null)
                    {
                        if (_strategyeMaps.ContainsKey(strategyId.Value))
                        {
                            throw new InvalidOperationException($"{nameof(GenerateXmlHandler)} cannot define more than one strategy to {strategyId.Value}");
                        }
                        else
                        {
                            _strategyeMaps.Add(strategyId.Value, item);
                        }
                    }
                }
            }

            return _strategyeMaps;
        }

        private static readonly IDictionary<ProductType, Type> _strategiesMap = InitializeStrategiesMap();

        public XDocument GenerateXml(ProductType productType)
        {
            if (!_strategiesMap.TryGetValue(productType, out Type strategyType))
            {
                throw new InvalidOperationException($"{nameof(GenerateXmlHandler)} could not find any strategy to { productType }");
            }

            var strategy = (IGenerateXmlStrategy)Activator.CreateInstance(strategyType);

            return strategy.GenerateXml(productType);
        }
    }
}