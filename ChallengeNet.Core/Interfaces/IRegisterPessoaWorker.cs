using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;

namespace ChallengeNet.Core.Interfaces
{
    public interface IRegisterPessoaWorker<T>
        where T : PessoaBase
    {
        Task<CoreResponse> Create(T pessoa);
    }
}
