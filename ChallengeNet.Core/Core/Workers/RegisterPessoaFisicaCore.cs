﻿using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Validator.Register;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public class RegisterPessoaFisicaCore : RegisterPessoaCoreBase<PessoaFisica, PessoaFisicaValidator>, IRegisterPessoaFisicaCore
    {
        public RegisterPessoaFisicaCore(IPessoaRepository<PessoaFisica> pessoaRepository, ILogger<RegisterPessoaFisicaCore> logger)
            : base(pessoaRepository, logger)
        {
        }

        public async Task<HttpResponse<PessoaFisica>> Find(string cpf)
        {
            try
            {
                var result = await PessoaRepository.Find(cpf);

                if (result == default)
                {
                    return HttpResponse<PessoaFisica>.AsNotFound($"{nameof(cpf)} not found");
                }

                return HttpResponse<PessoaFisica>.AsOk(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return HttpResponse<PessoaFisica>.AsError("Internal error, contact the administrator");
            }
        }

        protected override PessoaFisicaValidator GetValidatorClass()
        {
            return new PessoaFisicaValidator();
        }
    }
}