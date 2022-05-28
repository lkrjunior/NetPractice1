using ChallengeNet.Core.Models.User;
using ChallengeNet.Core.Models.Response;
using System.Threading.Tasks;

namespace ChallengeNet.Core.Interfaces
{
    public interface IAuthenticationCore
    {
        Task<HttpResponse> ExecuteAsync(User user);
    }
}
