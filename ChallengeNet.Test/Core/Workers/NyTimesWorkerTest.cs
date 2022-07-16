using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ChallengeNet.Core.Core.Workers;
using ChallengeNet.Core.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

namespace ChallengeNet.Test.Core.Workers
{
    public class NyTimesWorkerTest
    {
        [Fact]
        public async Task ShouldGetResultFromNyTimesApiWhenInvokeThis()
        {
            #region Arrange

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'success':true}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpClientFactory.Setup(_ => _.CreateClient(Consts.NyTimesHttpClient)).Returns(client);

            var appSettingsStub = new Dictionary<string, string>
            {
                {"NyTimesApi:ApiAddress", "http://localhost/"},
                {"NyTimesApi:ApiKey", "ApiKey"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            #endregion

            #region Act

            var worker = new NyTimesWorker(mockHttpClientFactory.Object, configuration);

            var result = await worker.ExecuteAsync();

            #endregion

            #region Assert

            Assert.False(result.HasError);
            Assert.NotNull(result.Data);

            #endregion

        }
    }
}

