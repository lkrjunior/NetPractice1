using ChallengeNet.Core.Models.User;
using ChallengeNet.Core.Models.Response;
using System.Threading.Tasks;

namespace ChallengeNet.Core.Interfaces
{
    public interface IAuthenticationWorker
    {
        Task<CoreResult<AuthenticationResponse>> ExecuteAsync(User user);
    }
}