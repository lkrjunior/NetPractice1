using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Exceptions;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;

namespace ChallengeNet.Core.Infrastructure
{
    public abstract class PessoaRepositoryBase<T> : IPessoaRepository<T>
        where T : PessoaBase
    {
        protected ICollection<T> Pessoas;

        public abstract Task<T> Find(string searchDoc);

        public async Task<T> Create(T pessoa)
        {
            try
            {
                pessoa.Id = Pessoas.Select(x => x.Id).FirstOrDefault() + 1;

                Pessoas.Add(pessoa);

                return await Task.FromResult(pessoa);
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception);
            }
        }

        public virtual async Task<T> Find(int id)
        {
            try
            {
                var result = Pessoas.FirstOrDefault(x => x.Id.Equals(id));

                return await Task.FromResult(result);
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception);
            }
        }
    }
}
