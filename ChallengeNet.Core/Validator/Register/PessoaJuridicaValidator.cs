using System;
using ChallengeNet.Core.Models.Register;
using FluentValidation;

namespace ChallengeNet.Core.Validator.Register
{
    public class PessoaJuridicaValidator : AbstractValidator<PessoaJuridica>
    {
        public PessoaJuridicaValidator()
        {
            Include(new PessoaBaseValidator());

            RuleFor(x => x.Cnpj).NotEmpty();
            RuleFor(x => x.Cnpj).NotNull();
        }
    }
}
