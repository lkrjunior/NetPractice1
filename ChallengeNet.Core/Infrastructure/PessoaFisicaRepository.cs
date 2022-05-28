using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Register;

namespace ChallengeNet.Core.Infrastructure
{
    public class PessoaFisicaRepository : PessoaRepositoryBase<PessoaFisica>
    {
        public PessoaFisicaRepository() : base()
        {
            Pessoas = new List<PessoaFisica>();
        }

        public override async Task<PessoaFisica> Find(string searchDoc)
        {
            var result = Pessoas.FirstOrDefault(x => x.Cpf.Equals(searchDoc));

            return await Task.FromResult(result);
        }
    }
}