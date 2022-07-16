using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Response;
using Microsoft.Extensions.Configuration;

namespace ChallengeNet.Core.Core.Workers
{
    public class NyTimesWorker : INyTimesWorker
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _apiAddress;

        public NyTimesWorker(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            var sectionNyTimes = configuration.GetSection("NyTimesApi").Get<IDictionary<string, string>>();
            _apiKey = sectionNyTimes["ApiKey"];
            _apiAddress = sectionNyTimes["ApiAddress"];
        }

        public async Task<CoreResult<string>> ExecuteAsync()
        {
            var client = _httpClientFactory.CreateClient(Consts.NyTimesHttpClient);

            var response = await client.GetAsync($"{_apiAddress}?api-key={_apiKey}");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();

                return CoreResult<string>.AsOk(responseJson);
            }

            return CoreResult<string>.AsError("NyTimesApi error");
        }
    }
}