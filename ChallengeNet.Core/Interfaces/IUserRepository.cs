using System.Threading.Tasks;
using ChallengeNet.Core.Models.User;

namespace ChallengeNet.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string userName, string password);
        Task<User> GetAsync(string userName);
    }
}