using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Exceptions;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Validator.Register;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public class RegisterPessoaFisicaWorker : RegisterPessoaWorkerBase<PessoaFisica, PessoaFisicaValidator>, IRegisterPessoaFisicaWorker
    {
        public RegisterPessoaFisicaWorker(IPessoaRepository<PessoaFisica> pessoaRepository, ILogger<RegisterPessoaFisicaWorker> logger)
            : base(pessoaRepository, logger)
        {
        }

        public async Task<CoreResult<PessoaFisica>> Find(string cpf)
        {
            try
            {
                var result = await PessoaRepository.Find(cpf);

                if (result == default)
                {
                    return CoreResult<PessoaFisica>.AsNotFound($"{nameof(cpf)} {Consts.ErrorNotFoundDescription}");
                }

                return CoreResult<PessoaFisica>.AsOk(result);
            }
            catch (RepositoryException repositoryException)
            {
                Logger.LogError(repositoryException.Message, typeof(RepositoryException), repositoryException);

                return CoreResult<PessoaFisica>.AsError(Consts.ErrorInternalServerDescription);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception.GetType(), exception);

                return CoreResult<PessoaFisica>.AsError(Consts.ErrorInternalServerDescription);
            }
        }

        protected override PessoaFisicaValidator GetValidatorClass()
        {
            return new PessoaFisicaValidator();
        }
    }
}
