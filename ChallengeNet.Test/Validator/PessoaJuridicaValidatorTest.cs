using System;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Validator.Register;
using FluentValidation.TestHelper;
using Xunit;

namespace ChallengeNet.Test.Validator
{
    public class PessoaJuridicaValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldValidateCnpjWhenCnpjIsEmptyOrNull(string cnpj)
        {
            #region Arrange

            var pessoa = new PessoaJuridica()
            {
                Name = "Name",
                Cnpj = cnpj
            };

            var pessoaJuridicaValidator = new PessoaJuridicaValidator();

            #endregion

            #region Act

            var validationResult = pessoaJuridicaValidator.TestValidate(pessoa);

            #endregion

            #region Assert

            var errorMessage = validationResult.ShouldHaveValidationErrorFor(x => x.Cnpj);

            Assert.NotNull(errorMessage);

            #endregion
        }
    }
}