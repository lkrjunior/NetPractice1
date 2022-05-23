using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChallengeNet.Core.Handlers
{
    public class UserTokenHandler : IUserTokenHandler
    {
        private readonly string _audience;
        private readonly double _expiresTokenInMinutes;
        private readonly string _issuer;
        private readonly string _secretKey;

        public UserTokenHandler(IConfiguration Configuration)
        {
            _secretKey = Configuration[Consts.SecretKey];
            _expiresTokenInMinutes = double.Parse(Configuration[Consts.ExpiresTokenInMinutes]);
            _issuer = Configuration[Consts.Issuer];
            _audience = Configuration[Consts.Audience];
        }

        public AuthenticationResponse GenerateAccessToken(User user)
        {
            var expires = DateTime.UtcNow.AddMinutes(_expiresTokenInMinutes);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_secretKey);

            var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Subject = subject,
                Expires = expires,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);

            var result = new AuthenticationResponse()
            {
                AccessToken = accessToken,
                ExpiresAtUtc = expires
            };

            return result;
        }
    }
}