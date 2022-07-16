using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeNet.API.Controllers
{
    [Route("api/[controller]")]
    public class NyTimesController : ControllerBase
    {
        private readonly INyTimesWorker _nyTimesWorker;
        
        public NyTimesController(INyTimesWorker nyTimesWorker)
        {
            _nyTimesWorker = nyTimesWorker;
        }

        [HttpGet("topstories")]
        public async Task<IActionResult> GetTopStories()
        {
            var result = await _nyTimesWorker.ExecuteAsync();

            if (result.HasError)
            {
                return Problem(result.ErrorMessage, HttpContext.Request.Path, result.HttpStatusCode, result.ErrorTitle);
            }

            return Ok(result.Data);
        }
    }
}

