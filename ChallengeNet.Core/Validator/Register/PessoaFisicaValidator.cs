using System;
using ChallengeNet.Core.Models.Register;
using FluentValidation;

namespace ChallengeNet.Core.Validator.Register
{
    public class PessoaFisicaValidator : AbstractValidator<PessoaFisica>
    {
        public PessoaFisicaValidator()
        {
            Include(new PessoaBaseValidator());

            RuleFor(x => x.Cpf).NotEmpty();
            RuleFor(x => x.Cpf).NotNull();
        }
    }
}
