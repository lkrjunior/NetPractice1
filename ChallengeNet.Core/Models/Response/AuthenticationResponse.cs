using System;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
