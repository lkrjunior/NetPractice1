using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Core.Workers;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Response;
using ChallengeNet.Core.Models.User;
using ChallengeNet.Test.Fake;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChallengeNet.Test.Core.Workers
{
    public class AuthenticationCoreTest
    {
        [Fact]
        public async Task ShouldCreateAccessTokenWhenUserIsValid()
        {
            #region Arrange

            var userName = "username";
            var password = "password";
            var role = "admin";

            var expectedAuthenticationResponse = new AuthenticationResponse()
            {
                AccessToken = Guid.NewGuid().ToString(),
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(10)
            };
            
            var user = UserFake.GenerateUser(userName, password);

            var userFromRepository = UserFake.GenerateUser(userName, password, role);

            var userRepository = new Mock<IUserRepository>();
            var currentUserName = default(string);
            var currentPassword = default(string);
            userRepository.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string userName, string password) =>
                {
                    currentUserName = userName;
                    currentPassword = password;

                    return userFromRepository;
                });

            var userTokenHandler = new Mock<IUserTokenHandler>();
            userTokenHandler.Setup(x => x.GenerateAccessToken(userFromRepository))
                .Returns(expectedAuthenticationResponse);

            var logger = new Mock<ILogger<AuthenticationCore>>();

            var authenticationCore = new AuthenticationCore(userRepository.Object, userTokenHandler.Object, logger.Object);

            #endregion

            #region Act

            var result = await authenticationCore.ExecuteAsync(user);

            #endregion

            #region Assert

            userRepository.Verify(x => x.GetAsync(userName, password), Times.Once);

            userTokenHandler.Verify(x => x.GenerateAccessToken(userFromRepository), Times.Once);

            Assert.Equal(currentUserName, userName);
            Assert.Equal(currentPassword, password);
            Assert.False(result.HasError);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedAuthenticationResponse, result.Data);

            #endregion
        }

        [Fact]
        public async Task ShouldNotCreateAccessTokenWhenUserIsInvalid()
        {
            #region Arrange

            var userName = "username";
            var password = "password";
            var expectedAccessToken = Guid.NewGuid().ToString();
            var expectedStatusCode = StatusCodes.Status400BadRequest;

            var user = UserFake.GenerateUser(userName, password);

            var userFromRepository = default(User);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(userName, password)).ReturnsAsync(userFromRepository);

            var userTokenHandler = new Mock<IUserTokenHandler>();
            
            var logger = new Mock<ILogger<AuthenticationCore>>();

            var authenticationCore = new AuthenticationCore(userRepository.Object, userTokenHandler.Object, logger.Object);

            #endregion

            #region Act

            var result = await authenticationCore.ExecuteAsync(user);

            #endregion

            #region Assert

            userRepository.Verify(x => x.GetAsync(userName, password), Times.Once);

            userTokenHandler.Verify(x => x.GenerateAccessToken(userFromRepository), Times.Never);

            Assert.True(result.HasError);
            Assert.Equal(expectedStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.ErrorMessage);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnInternalServerErrorWhenThrowsAnException()
        {
            #region Arrange

            var userName = "username";
            var password = "password";
            var expectedAccessToken = Guid.NewGuid().ToString();
            var expectedStatusCode = StatusCodes.Status500InternalServerError;

            var user = UserFake.GenerateUser(userName, password);

            var userFromRepository = default(User);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(userName, password)).Throws(new Exception("Cannot access repository"));

            var userTokenHandler = new Mock<IUserTokenHandler>();
            
            var logger = new Mock<ILogger<AuthenticationCore>>();

            var authenticationCore = new AuthenticationCore(userRepository.Object, userTokenHandler.Object, logger.Object);

            #endregion

            #region Act

            var result = await authenticationCore.ExecuteAsync(user);

            #endregion

            #region Assert

            userRepository.Verify(x => x.GetAsync(userName, password), Times.Once);

            userTokenHandler.Verify(x => x.GenerateAccessToken(userFromRepository), Times.Never);

            logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.ErrorMessage);

            #endregion
        }
    }
}
