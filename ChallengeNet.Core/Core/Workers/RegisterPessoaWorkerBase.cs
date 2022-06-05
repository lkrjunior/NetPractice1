using System;
using System.Text;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public abstract class RegisterPessoaWorkerBase<TEntity, TValidatorClass>
        where TEntity : PessoaBase
        where TValidatorClass : AbstractValidator<TEntity>
    {
        private static string GetErrorMessageFromValidation(FluentValidation.Results.ValidationResult validationResult)
        {
            var messageBuilder = new StringBuilder();

            foreach (var errorValidation in validationResult.Errors)
            {
                if (messageBuilder.Length > 0)
                {
                    messageBuilder.Append(", ");
                }

                messageBuilder.Append(errorValidation);
            }

            return messageBuilder.ToString();
        }

        protected readonly IPessoaRepository<TEntity> PessoaRepository;
        protected readonly ILogger<RegisterPessoaWorkerBase<TEntity, TValidatorClass>> Logger;

        protected RegisterPessoaWorkerBase(IPessoaRepository<TEntity> pessoaRepository, ILogger<RegisterPessoaWorkerBase<TEntity, TValidatorClass>> logger)
        {
            PessoaRepository = pessoaRepository;
            Logger = logger;
        }

        public async Task<CoreResult<TEntity>> Create(TEntity pessoa)
        {
            try
            {
                var validator = GetValidatorClass();

                var validationResult = validator.Validate(pessoa);

                if (!validationResult.IsValid)
                {
                    var errorsMessage = GetErrorMessageFromValidation(validationResult);

                    return CoreResult<TEntity>.AsBadRequest(errorsMessage);
                }

                await PessoaRepository.Create(pessoa);

                return CoreResult<TEntity>.AsOk(pessoa);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);

                return CoreResult<TEntity>.AsError(Consts.ErrorInternalServerDescription);
            }
        }

        protected abstract TValidatorClass GetValidatorClass();
    }
}
