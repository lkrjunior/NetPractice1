using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;

namespace ChallengeNet.Core.Infrastructure
{
    public abstract class PessoaRepositoryBase<T> : IPessoaRepository<T>
        where T : PessoaBase
    {
        protected ICollection<T> Pessoas;

        public async Task<bool> Create(T pessoa)
        {
            Pessoas.Add(pessoa);

            pessoa.Id = Pessoas.Select(x => x.Id).FirstOrDefault() + 1;

            return await Task.FromResult(true);
        }

        public abstract Task<T> Find(string searchDoc);

        public virtual async Task<T> Find(int id)
        {
            var result = Pessoas.FirstOrDefault(x => x.Id.Equals(id));

            return await Task.FromResult(result);
        }
    }
}
