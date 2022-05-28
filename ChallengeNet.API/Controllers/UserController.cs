using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.User;
using ChallengeNet.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChallengeNet.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChallengeNet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAuthenticationCore _authenticationCore;

        public UserController(ILogger<UserController> logger, IAuthenticationCore authenticationCore)
        {
            _logger = logger;
            _authenticationCore = authenticationCore;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] User user)
        {
            var result = await _authenticationCore.ExecuteAsync(user);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
