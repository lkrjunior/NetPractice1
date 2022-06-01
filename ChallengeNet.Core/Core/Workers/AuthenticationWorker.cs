using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Models.User;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public class AuthenticationWorker : IAuthenticationWorker
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenHandler _authenticationHandler;
        private readonly ILogger<AuthenticationWorker> _logger;

        public AuthenticationWorker(IUserRepository userRepository, IUserTokenHandler authenticationHandler, ILogger<AuthenticationWorker> logger)
        {
            _userRepository = userRepository;
            _authenticationHandler = authenticationHandler;
            _logger = logger;
        }

        public async Task<CoreResponse> ExecuteAsync(User user)
        {
            try
            {
                var userFromRepository = await _userRepository.GetAsync(user.UserName);

                if (userFromRepository == default)
                {
                    return CoreResponse.AsBadRequest(Consts.ErrorUserAndOrPasswordInvalidDescription);
                }

                var authenticationResponse = _authenticationHandler.GenerateAccessToken(userFromRepository);

                return CoreResponse.AsOk(authenticationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return CoreResponse.AsError(Consts.ErrorInternalServerDescription);
            }
        }
    }
}