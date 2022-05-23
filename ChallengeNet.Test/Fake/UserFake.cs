using ChallengeNet.Core.Models.User;

namespace ChallengeNet.Test.Fake
{
    public static class UserFake
    {
        public static User GenerateUser(string userName, string password, string role = default)
        {
            var user = new User()
            {
                UserName = userName,
                Password = password,
                Role = role
            };

            return user;
        }
    }
}
