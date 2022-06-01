using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Core.Workers;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChallengeNet.Test.Core.Workers
{
    public class RegisterPessoaJuridicaWorkerTest
    {
        [Fact]
        public async Task ShouldCreatePessoaJuridicaWhenPayloadIsValid()
        {
            #region Arrange

            var pessoa = new PessoaJuridica()
            {
                Name = "Name",
                Cnpj = "12345678901234"
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaJuridica>>();
            repositoryMock.Setup(x => x.Create(It.IsAny<PessoaJuridica>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<RegisterPessoaJuridicaWorker>>();

            var worker = new RegisterPessoaJuridicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaJuridica>()), Times.Once);

            Assert.False(result.HasError);
            Assert.NotNull(result.Data);

            #endregion
        }

        [Fact]
        public async Task ShouldNotCreatePessoaJuridicaWhenPayloadIsInvalid()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status400BadRequest;

            var pessoa = new PessoaJuridica();

            var repositoryMock = new Mock<IPessoaRepository<PessoaJuridica>>();

            var loggerMock = new Mock<ILogger<RegisterPessoaJuridicaWorker>>();

            var worker = new RegisterPessoaJuridicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaJuridica>()), Times.Never);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.ErrorMessage);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnsInternalServerErrorOnCreateWhenThrowsAnExceprion()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status500InternalServerError;

            var pessoa = new PessoaJuridica()
            {
                Name = "Name",
                Cnpj = "12345678901234"
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaJuridica>>();
            repositoryMock.Setup(x => x.Create(It.IsAny<PessoaJuridica>())).ThrowsAsync(new Exception());

            var loggerMock = new Mock<ILogger<RegisterPessoaJuridicaWorker>>();

            var worker = new RegisterPessoaJuridicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Create(pessoa);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaJuridica>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.ErrorMessage);

            #endregion
        }

        [Fact]
        public async Task ShouldFindPessoaJuridicaWhenSearchIsCorrect()
        {
            #region Arrange

            var cnpj = "12345678901234";

            var pessoa = new PessoaJuridica()
            {
                Name = "Name",
                Cnpj = cnpj
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaJuridica>>();
            repositoryMock.Setup(x => x.Find(It.IsAny<string>()))
                .ReturnsAsync(pessoa);

            var loggerMock = new Mock<ILogger<RegisterPessoaJuridicaWorker>>();

            var worker = new RegisterPessoaJuridicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cnpj);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaJuridica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.False(result.HasError);
            Assert.NotNull(result.Data);

            #endregion
        }

        [Fact]
        public async Task ShouldNotFindPessoaJuridicaWhenSearchIsIncorrect()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status404NotFound;

            var cnpj = "12345678901234";

            var pessoa = new PessoaJuridica()
            {
                Name = "Name",
                Cnpj = cnpj
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaJuridica>>();

            var loggerMock = new Mock<ILogger<RegisterPessoaJuridicaWorker>>();

            var worker = new RegisterPessoaJuridicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cnpj);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaJuridica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.ErrorMessage);

            #endregion
        }

        [Fact]
        public async Task ShouldReturnsInternalServerErrorOnFindWhenThrowsAnException()
        {
            #region Arrange

            var expectedHttpStatusCode = StatusCodes.Status500InternalServerError;

            var cnpj = "12345678901234";

            var pessoa = new PessoaJuridica()
            {
                Name = "Name",
                Cnpj = cnpj
            };

            var repositoryMock = new Mock<IPessoaRepository<PessoaJuridica>>();
            repositoryMock.Setup(x => x.Find(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var loggerMock = new Mock<ILogger<RegisterPessoaJuridicaWorker>>();

            var worker = new RegisterPessoaJuridicaWorker(repositoryMock.Object, loggerMock.Object);
            #endregion

            #region Act

            var result = await worker.Find(cnpj);

            #endregion

            #region Assert

            repositoryMock.Verify(x => x.Create(It.IsAny<PessoaJuridica>()), Times.Never);
            repositoryMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);

            Assert.True(result.HasError);
            Assert.Equal(expectedHttpStatusCode, result.HttpStatusCode);
            Assert.NotNull(result.ErrorMessage);

            #endregion
        }
    }
}