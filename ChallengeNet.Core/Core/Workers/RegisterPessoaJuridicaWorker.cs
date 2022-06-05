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
    public class RegisterPessoaJuridicaWorker : RegisterPessoaWorkerBase<PessoaJuridica, PessoaJuridicaValidator>, IRegisterPessoaJuridicaWorker
    {
        public RegisterPessoaJuridicaWorker(IPessoaRepository<PessoaJuridica> pessoaRepository, ILogger<RegisterPessoaJuridicaWorker> logger)
            : base(pessoaRepository, logger)
        {
        }

        public async Task<CoreResult<PessoaJuridica>> Find(string cnpj)
        {
            try
            {
                var result = await PessoaRepository.Find(cnpj);

                if (result == default)
                {
                    return CoreResult<PessoaJuridica>.AsNotFound($"{nameof(cnpj)} {Consts.ErrorNotFoundDescription}");
                }

                return CoreResult<PessoaJuridica>.AsOk(result);
            }
            catch (RepositoryException repositoryException)
            {
                Logger.LogError(repositoryException.Message, typeof(RepositoryException), repositoryException);

                return CoreResult<PessoaJuridica>.AsError(Consts.ErrorInternalServerDescription);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception.GetType(), exception);

                return CoreResult<PessoaJuridica>.AsError(Consts.ErrorInternalServerDescription);
            }
        }

        protected override PessoaJuridicaValidator GetValidatorClass()
        {
            return new PessoaJuridicaValidator();
        }
    }
}
