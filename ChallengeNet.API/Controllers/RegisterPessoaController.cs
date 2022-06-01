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
        private readonly IRegisterPessoaFisicaWorker _registerPessoaFisicaWorker;
        private readonly IRegisterPessoaJuridicaWorker _registerPessoaJuridicaWorker;

        public RegisterPessoaController(IRegisterPessoaFisicaWorker registerPessoaFisicaWorker, IRegisterPessoaJuridicaWorker registerPessoaJuridicaWorker)
        {
            _registerPessoaFisicaWorker = registerPessoaFisicaWorker;
            _registerPessoaJuridicaWorker = registerPessoaJuridicaWorker;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("pessoafisica/create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PessoaFisica))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePessoaFisica([FromBody] PessoaFisica pessoa)
        {
            var result = await _registerPessoaFisicaWorker.Create(pessoa);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("pessoafisica/find")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PessoaFisica))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindPessoaFisica([FromQuery] string cpf)
        {
            var result = await _registerPessoaFisicaWorker.Find(cpf);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("pessoajuridica/create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PessoaJuridica))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePessoaJuridica([FromBody] PessoaJuridica pessoa)
        {
            var result = await _registerPessoaJuridicaWorker.Create(pessoa);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("pessoajuridica/find")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PessoaJuridica))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindPessoaJuridica([FromQuery] string cnpj)
        {
            var result = await _registerPessoaJuridicaWorker.Find(cnpj);

            if (result.HasError)
            {
                return StatusCode(result.HttpStatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}