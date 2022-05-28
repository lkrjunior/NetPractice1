using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Validator.Register;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.Core.Core.Workers
{
    public class RegisterPessoaJuridicaCore : RegisterPessoaCoreBase<PessoaJuridica, PessoaJuridicaValidator>, IRegisterPessoaJuridicaCore
    {
        public RegisterPessoaJuridicaCore(IPessoaRepository<PessoaJuridica> pessoaRepository, ILogger<RegisterPessoaJuridicaCore> logger)
            : base(pessoaRepository, logger)
        {
        }

        public async Task<HttpResponse> Find(string cnpj)
        {
            try
            {
                var result = await PessoaRepository.Find(cnpj);

                if (result == default)
                {
                    return HttpResponse.AsNotFound($"{nameof(cnpj)} not found");
                }

                return HttpResponse.AsOk(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);

                return HttpResponse.AsError("Internal error, contact the administrator");
            }
        }

        protected override PessoaJuridicaValidator GetValidatorClass()
        {
            return new PessoaJuridicaValidator();
        }
    }
}
