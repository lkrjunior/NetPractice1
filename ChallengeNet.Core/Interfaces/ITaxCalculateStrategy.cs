using System;
using ChallengeNet.Core.Models;

namespace ChallengeNet.Core.Interfaces
{
    public interface ITaxCalculateStrategy : ITaxCalculateHandler
    {
        ProductType ProductType { get; }
    }
}
