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
    /// <summary>
    /// https://developer.nytimes.com
    /// </summary>
    public class NyTimesWorker : INyTimesWorker
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _apiAddress;

        public NyTimesWorker(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            _apiKey = configuration[$"{Consts.NyTimesApiSection}:{Consts.NyTimesApiKey}"];
            _apiAddress = configuration[$"{Consts.NyTimesApiSection}:{Consts.NyTimesApiAddress}"];
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