using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.User;

namespace ChallengeNet.Core.Infrastructure
{
    /// <summary>
    /// Implementation for Repository
    /// </summary>
    public class UserRepository : IUserRepository
    {

        private readonly IEnumerable<User> _users;

        public UserRepository()
        {
            _users = InitializeRepository();
        }

        private static IEnumerable<User> InitializeRepository()
        {
            var users = new User[]
            {
                new User { UserName = "admin", Password = "admin", Role = Consts.RuleAdmin },
                new User { UserName = "user", Password = "user", Role = Consts.RuleUser },
            };

            return users;
        }

        public async Task<User> GetAsync(string userName, string password)
        {
            var user = _users.FirstOrDefault(x => x.UserName.Equals(userName) && x.Password.Equals(password));

            return await Task.Run(() => user);
        }
    }
}