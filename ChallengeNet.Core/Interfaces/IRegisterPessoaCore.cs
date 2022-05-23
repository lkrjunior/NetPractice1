using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;

namespace ChallengeNet.Core.Interfaces
{
    public interface IRegisterPessoaCore<T>
        where T : PessoaBase
    {
        Task<HttpResponse<T>> Create(T pessoa);
    }
}
