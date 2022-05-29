using System;

namespace ChallengeNet.Core.Models.AttributeStrategy
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ProductTypeStrategyAttribute : Attribute
    {
        public ProductTypeStrategyAttribute(ProductType value)
        {
            Value = value;
        }

        public ProductType Value { get; private set; }
    }
}
