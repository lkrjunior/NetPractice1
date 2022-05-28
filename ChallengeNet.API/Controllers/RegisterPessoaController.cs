using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeNet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterPessoaController : ControllerBase
    {
        private readonly IRegisterPessoaFisicaCore _registerPessoaFisicaCore;
        private readonly IRegisterPessoaJuridicaCore _registerPessoaJuridicaCore;

        public RegisterPessoaController(IRegisterPessoaFisicaCore registerPessoaFisicaCore, IRegisterPessoaJuridicaCore registerPessoaJuridicaCore)
        {
            _registerPessoaFisicaCore = registerPessoaFisicaCore;
            _registerPessoaJuridicaCore = registerPessoaJuridicaCore;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("pessoafisica/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PessoaFisica>> CreatePessoaFisica([FromBody] PessoaFisica pessoa)
        {
            var result = await _registerPessoaFisicaCore.Create(pessoa);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("pessoafisica/find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PessoaFisica>> FindPessoaFisica([FromQuery] string cpf)
        {
            var result = await _registerPessoaFisicaCore.Find(cpf);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("pessoajuridica/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PessoaJuridica>> CreatePessoaJuridica([FromBody] PessoaJuridica pessoa)
        {
            var result = await _registerPessoaJuridicaCore.Create(pessoa);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("pessoajuridica/find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PessoaJuridica>> FindPessoaJuridica([FromQuery] string cnpj)
        {
            var result = await _registerPessoaJuridicaCore.Find(cnpj);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}