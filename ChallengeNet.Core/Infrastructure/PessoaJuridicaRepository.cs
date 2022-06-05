using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Exceptions;
using ChallengeNet.Core.Models.Register;

namespace ChallengeNet.Core.Infrastructure
{
    public class PessoaJuridicaRepository : PessoaRepositoryBase<PessoaJuridica>
    {
        public PessoaJuridicaRepository() : base()
        {
            Pessoas = new List<PessoaJuridica>();
        }

        public override async Task<PessoaJuridica> Find(string searchDoc)
        {
            try
            {
                var result = Pessoas.FirstOrDefault(x => x.Cnpj.Equals(searchDoc));

                return await Task.FromResult(result);
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception);
            }
        }
    }
}
