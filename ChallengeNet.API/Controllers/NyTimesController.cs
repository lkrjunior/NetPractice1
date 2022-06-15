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
        private readonly string _baseAddress;
        private readonly string _apiTopStories;

        public NyTimesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            var sectionNyTimes = configuration.GetSection("NyTimesApi").Get<IDictionary<string, string>>();
            _apiKey = sectionNyTimes["ApiKey"];
            _baseAddress = sectionNyTimes["BaseAddress"];
            _apiTopStories = sectionNyTimes["ApiTopStories"];
        }

        [HttpGet("topstories")]
        public async Task<IActionResult> GetTopStories()
        {
            var client = _httpClientFactory.CreateClient();
                
            client.BaseAddress = new Uri(_baseAddress);

            var response = await client.GetAsync($"{_apiTopStories}?api-key={_apiKey}");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();

                return Ok(responseJson);
            }

            return BadRequest();

        }

    }
}

