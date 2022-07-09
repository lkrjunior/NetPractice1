using System;
using System.Text;
using System.Threading.Tasks;
using ChallengeNet.Core.Exceptions;
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
            var message = string.Join(" ", validationResult.Errors);

            return message;
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

                var result = await PessoaRepository.Create(pessoa);

                return CoreResult<TEntity>.AsOk(result);
            }
            catch (RepositoryException repositoryException)
            {
                Logger.LogError(repositoryException.Message, typeof(RepositoryException), repositoryException);

                return CoreResult<TEntity>.AsError(Consts.ErrorInternalServerDescription);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception.GetType(), exception);

                return CoreResult<TEntity>.AsError(Consts.ErrorInternalServerDescription);
            }
        }

        protected abstract TValidatorClass GetValidatorClass();
    }
}
