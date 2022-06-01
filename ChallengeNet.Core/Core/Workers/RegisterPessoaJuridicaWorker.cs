using System;
using System.Threading.Tasks;
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

        public async Task<CoreResponse> Find(string cnpj)
        {
            try
            {
                var result = await PessoaRepository.Find(cnpj);

                if (result == default)
                {
                    return CoreResponse.AsNotFound($"{nameof(cnpj)} {Consts.ErrorNotFoundDescription}");
                }

                return CoreResponse.AsOk(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);

                return CoreResponse.AsError(Consts.ErrorInternalServerDescription);
            }
        }

        protected override PessoaJuridicaValidator GetValidatorClass()
        {
            return new PessoaJuridicaValidator();
        }
    }
}
