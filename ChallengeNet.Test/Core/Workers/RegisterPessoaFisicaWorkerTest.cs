using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Core.Workers;
using ChallengeNet.Core.Exceptions;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChallengeNet.Test.Core.Workers
{
    public class RegisterPessoaFisicaWorkerTest
    {
        [Fact]
        public async Task ShouldCreatePessoaFisicaWhenPayloadIsValid()
        {
            #region Arrange

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = "12345678901"
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();
            repositoryMock.Setup(x => x.Create(It.IsAny<PessoaFisica>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);

            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Once);

            Assert.False(result.HasError);
            Assert.NotNull(result.Data);

            #endregion
        }

        [Fact]
        public async Task ShouldNotCreatePessoaFisicaWhenPayloadIsInvalid()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status400BadRequest;

            var pessoa = new PessoaFisica();
            
            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Never);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.Error);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnsInternalServerErrorOnCreateWhenThrowsAnException()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status500InternalServerError;

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = "12345678901"
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();
            repositoryMock.Setup(x => x.Create(It.IsAny<PessoaFisica>())).ThrowsAsync(new Exception());

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);

            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.Error);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnsInternalServerErrorOnCreateWhenThrowsAnRepositoryException()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status500InternalServerError;

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = "12345678901"
            };

            var innerException = new Exception();
            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();
            repositoryMock.Setup(x => x.Create(It.IsAny<PessoaFisica>())).ThrowsAsync(new RepositoryException(innerException));

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);

            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.Error);

            #endregion
        }

        [Fact]
        public async Task ShouldFindPessoaFisicaWhenSearchIsCorrect()
        {
            #region Arrange

            var cpf = "12345678901";

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = cpf
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();
            repositoryMock.Setup(x => x.Find(It.IsAny<string>()))
                .ReturnsAsync(pessoa);

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cpf);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.False(result.HasError);
            Assert.NotNull(result.Data);

            #endregion
        }

        [Fact]
        public async Task ShouldNotFindPessoaFisicaWhenSearchIsIncorrect()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status404NotFound;

            var cpf = "12345678901";

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = cpf
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cpf);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.Error);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnsInternalServerErrorOnFindWhenSearchIsCorrect()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status500InternalServerError;

            var cpf = "12345678901";

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = cpf
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();
            repositoryMock.Setup(x => x.Find(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cpf);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.Error);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnsInternalServerErrorOnFindWhenSearchReturnsException()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status500InternalServerError;

            var cpf = "12345678901";

            var pessoa = new PessoaFisica()
            {
                Name = "Name",
                Cpf = cpf
            };

            var innerException = new Exception();
            var repositoryMock = new Mock<IPessoaRepository<PessoaFisica>>();
            repositoryMock.Setup(x => x.Find(It.IsAny<string>()))
                .ThrowsAsync(new RepositoryException(innerException));

            var loggerMock = new Mock<ILogger<RegisterPessoaFisicaWorker>>();

            var worker = new RegisterPessoaFisicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cpf);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaFisica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.Error);

            #endregion
        }
    }
}
