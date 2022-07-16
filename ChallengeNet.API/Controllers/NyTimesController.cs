using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChallengeNet.API.Controllers
{
    [Route("api/[controller]")]
    public class NyTimesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _apiAddress;

        public NyTimesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            var sectionNyTimes = configuration.GetSection("NyTimesApi").Get<IDictionary<string, string>>();
            _apiKey = sectionNyTimes["ApiKey"];
            _apiAddress = sectionNyTimes["ApiAddress"];
        }

        [HttpGet("topstories")]
        public async Task<IActionResult> GetTopStories()
        {
            var client = _httpClientFactory.CreateClient(nameof(NyTimesController));
            
            var response = await client.GetAsync($"{_apiAddress}?api-key={_apiKey}");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();

                return Ok(responseJson);
            }

            return BadRequest();

        }

    }
}

