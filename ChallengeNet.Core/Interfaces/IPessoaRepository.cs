using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Register;

namespace ChallengeNet.Core.Interfaces
{
    public interface IPessoaRepository<T>
        where T : PessoaBase
    {
        Task<bool> Create(T pessoa);
        Task<T> Find(string searchDoc);
        Task<T> Find(int id);
    }
}
