using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Models.User;

namespace ChallengeNet.Core.Interfaces
{
    public interface IUserTokenHandler
    {
        AuthenticationResponse GenerateAccessToken(User user);
    }
}
