using System;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Validator.Register;
using FluentValidation.TestHelper;
using Xunit;

namespace ChallengeNet.Test.Validator
{
    public class PessoaFisicaValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldValidateCpfWhenCpfIsEmptyOrNull(string cpf)
        {
            #region Arrange

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = cpf
            };

            var pessoaFisicaValidator = new PessoaFisicaValidator();

            #endregion

            #region Act

            var validationResult = pessoaFisicaValidator.TestValidate(pessoa);

            #endregion

            #region Assert

            var errorMessage = validationResult.ShouldHaveValidationErrorFor(x => x.Cpf);

            Assert.NotNull(errorMessage);

            #endregion
        }
    }
}