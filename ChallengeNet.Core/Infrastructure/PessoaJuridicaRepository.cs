using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var result = Pessoas.FirstOrDefault(x => x.Cnpj.Equals(searchDoc));

            return await Task.Run(() => result);
        }
    }
}
