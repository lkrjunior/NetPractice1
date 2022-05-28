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
    public abstract class RegisterPessoaCoreBase<TEntity, TValidatorClass>
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
        protected readonly ILogger<RegisterPessoaCoreBase<TEntity, TValidatorClass>> Logger;

        protected RegisterPessoaCoreBase(IPessoaRepository<TEntity> pessoaRepository, ILogger<RegisterPessoaCoreBase<TEntity, TValidatorClass>> logger)
        {
            PessoaRepository = pessoaRepository;
            Logger = logger;
        }

        public async Task<HttpResponse> Create(TEntity pessoa)
        {
            try
            {
                var validator = GetValidatorClass();

                var validationResult = validator.Validate(pessoa);

                if (!validationResult.IsValid)
                {
                    var errorsMessage = GetErrorMessageFromValidation(validationResult);

                    return HttpResponse.AsBadRequest(errorsMessage);
                }

                await PessoaRepository.Create(pessoa);

                return HttpResponse.AsOk(pessoa);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);

                return HttpResponse.AsError(Consts.ErrorInternalServerDescription);
            }
        }

        protected abstract TValidatorClass GetValidatorClass();
    }
}
