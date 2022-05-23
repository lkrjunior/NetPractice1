﻿using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public abstract class RegisterPessoaCoreBase<TEntity, TValidatorClass>
        where TEntity : PessoaBase
        where TValidatorClass : AbstractValidator<TEntity>
    {
        private string GetErrorMessageFromValidation(FluentValidation.Results.ValidationResult validationResult)
        {
            var message = default(string);

            foreach (var errorValidation in validationResult.Errors)
            {
                message += $"{(message != default ? ", " : "")}{errorValidation}";
            }

            return message;
        }

        protected readonly IPessoaRepository<TEntity> PessoaRepository;
        protected readonly ILogger<RegisterPessoaCoreBase<TEntity, TValidatorClass>> _logger;

        public RegisterPessoaCoreBase(IPessoaRepository<TEntity> pessoaRepository, ILogger<RegisterPessoaCoreBase<TEntity, TValidatorClass>> logger)
        {
            PessoaRepository = pessoaRepository;
            _logger = logger;
        }

        public async Task<HttpResponse<TEntity>> Create(TEntity pessoa)
        {
            try
            {
                var validator = GetValidatorClass();

                var validationResult = validator.Validate(pessoa);

                if (!validationResult.IsValid)
                {
                    var errorsMessage = GetErrorMessageFromValidation(validationResult);

                    return HttpResponse<TEntity>.AsBadRequest(errorsMessage);
                }

                await PessoaRepository.Create(pessoa);

                return HttpResponse<TEntity>.AsOk(pessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return HttpResponse<TEntity>.AsError("Internal error, contact the administrator");
            }
        }

        protected abstract TValidatorClass GetValidatorClass();
    }
}