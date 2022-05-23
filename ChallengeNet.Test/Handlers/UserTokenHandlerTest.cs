using System;
using ChallengeNet.Core.Handlers;
using ChallengeNet.Core.Models;
using ChallengeNet.Test.Fake;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ChallengeNet.Test.Handlers
{
    public class UserTokenHandlerTest
    {
        [Fact]
        public void ShouldGenerateAccessTokenWhenUserIsValid()
        {
            #region Arrange

            var user = UserFake.GenerateUser("username", "password", "admin");

            var secret = Guid.NewGuid().ToString();

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x[Consts.SecretKey]).Returns(secret);
            configuration.Setup(x => x[Consts.ExpiresTokenInMinutes]).Returns("2");
            configuration.Setup(x => x[Consts.Issuer]).Returns("issuer");
            configuration.Setup(x => x[Consts.Audience]).Returns("audience");

            var userTokenHandler = new UserTokenHandler(configuration.Object);

            #endregion

            #region Act

            var result = userTokenHandler.GenerateAccessToken(user);

            #endregion

            #region Assert

            configuration.Verify(x => x[Consts.SecretKey], Times.Once);
            configuration.Verify(x => x[Consts.ExpiresTokenInMinutes], Times.Once);
            configuration.Verify(x => x[Consts.Issuer], Times.Once);
            configuration.Verify(x => x[Consts.Audience], Times.Once);

            Assert.NotNull(result.AccessToken);
            Assert.True(result.ExpiresAtUtc > DateTime.UtcNow);

            #endregion
        }

        [Fact]
        public void ShouldThrowExceptionWhenSecretIsInvalid()
        {
            #region Arrange

            var user = UserFake.GenerateUser("username", "password", "admin");

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x[Consts.SecretKey]).Returns("secret");
            configuration.Setup(x => x[Consts.ExpiresTokenInMinutes]).Returns("2");
            configuration.Setup(x => x[Consts.Issuer]).Returns("issuer");
            configuration.Setup(x => x[Consts.Audience]).Returns("audience");

            var userTokenHandler = new UserTokenHandler(configuration.Object);

            #endregion

            #region Act

            Action action = () => userTokenHandler.GenerateAccessToken(user);

            #endregion

            #region Assert

            configuration.Verify(x => x[Consts.SecretKey], Times.Once);
            configuration.Verify(x => x[Consts.ExpiresTokenInMinutes], Times.Once);
            configuration.Verify(x => x[Consts.Issuer], Times.Once);
            configuration.Verify(x => x[Consts.Audience], Times.Once);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.NotNull(exception.Message);

            #endregion
        }
    }
}