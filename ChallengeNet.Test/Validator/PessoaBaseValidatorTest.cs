using System;
using ChallengeNet.Core.Models.Register;
using ChallengeNet.Core.Validator.Register;
using FluentValidation.TestHelper;
using Xunit;

namespace ChallengeNet.Test.Validator
{
    public class PessoaBaseValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldValidateNameWhenNameIsEmptyOrNull(string name)
        {
            #region Arrange

            var pessoa = new PessoaFisica()
            {
                Name = name,
            };

            var pessoaBaseValidator = new PessoaBaseValidator();

            #endregion

            #region Act

            var validationResult = pessoaBaseValidator.TestValidate(pessoa);

            #endregion

            #region Assert

            var errorMessage = validationResult.ShouldHaveValidationErrorFor(x => x.Name);

            Assert.NotNull(errorMessage);

            #endregion
        }
    }
}