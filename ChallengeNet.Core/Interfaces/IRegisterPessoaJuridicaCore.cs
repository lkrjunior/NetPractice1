using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;

namespace ChallengeNet.Core.Interfaces
{
    public interface IRegisterPessoaJuridicaCore : IRegisterPessoaCore<PessoaJuridica>
    {
        Task<HttpResponse> Find(string cnpj);
    }
}
