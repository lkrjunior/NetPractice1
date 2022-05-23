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

        public async Task<HttpResponse<PessoaJuridica>> Find(string cnpj)
        {
            try
            {
                var result = await PessoaRepository.Find(cnpj);

                if (result == default)
                {
                    return HttpResponse<PessoaJuridica>.AsNotFound($"{nameof(cnpj)} not found");
                }

                return HttpResponse<PessoaJuridica>.AsOk(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return HttpResponse<PessoaJuridica>.AsError("Internal error, contact the administrator");
            }
        }

        protected override PessoaJuridicaValidator GetValidatorClass()
        {
            return new PessoaJuridicaValidator();
        }
    }
}
