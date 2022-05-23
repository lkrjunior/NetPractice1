using System;
using ChallengeNet.Core.Models.Register;
using FluentValidation;

namespace ChallengeNet.Core.Validator.Register
{
    public class PessoaBaseValidator : AbstractValidator<PessoaBase>
    {
        public PessoaBaseValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).NotNull();
        }
    }
}
