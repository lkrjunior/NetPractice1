﻿using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Models.User;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public class AuthenticationCore : IAuthenticationCore
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenHandler _authenticationHandler;
        private readonly ILogger<AuthenticationCore> _logger;

        public AuthenticationCore(IUserRepository userRepository, IUserTokenHandler authenticationHandler, ILogger<AuthenticationCore> logger)
        {
            _userRepository = userRepository;
            _authenticationHandler = authenticationHandler;
            _logger = logger;
        }

        public async Task<HttpResponse<AuthenticationResponse>> ExecuteAsync(User user)
        {
            try
            {
                var userFromRepository = await _userRepository.GetAsync(user.UserName, user.Password);

                if (userFromRepository == default)
                {
                    return HttpResponse<AuthenticationResponse>.AsBadRequest("User or password invalid");
                }

                var authenticationResponse = _authenticationHandler.GenerateAccessToken(userFromRepository);

                return HttpResponse<AuthenticationResponse>.AsOk(authenticationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return HttpResponse<AuthenticationResponse>.AsError("Internal error, contact the administrator");
            }
        }
    }
}