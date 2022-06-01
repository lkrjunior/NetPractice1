using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;

namespace ChallengeNet.Core.Interfaces
{
    public interface IRegisterPessoaFisicaWorker : IRegisterPessoaWorker<PessoaFisica>
    {
        Task<CoreResponse> Find(string cpf);
    }
}
