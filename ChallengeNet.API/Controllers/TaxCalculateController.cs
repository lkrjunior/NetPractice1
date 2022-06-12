using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxCalculateController : ControllerBase
    {
        private readonly ILogger<TaxCalculateController> _logger;
        private readonly Func<ProductType, ITaxCalculateStrategy> _funcTaxCalculateStrategy;
        private readonly ITaxCalculateWithFuncHandler _taxCalculateWithFuncHandler;
        private readonly IEnumerable<ITaxCalculateStrategy> _taxCalculateStrategies;

        private void LogInformation<T>(T result)
        {
            _logger.LogInformation($"Result {result}");
        }

        public TaxCalculateController(ILogger<TaxCalculateController> logger, Func<ProductType, ITaxCalculateStrategy> funcTaxCalculateStrategy, ITaxCalculateWithFuncHandler taxCalculateWithFuncHandler, IEnumerable<ITaxCalculateStrategy> taxCalculateStrategies)
        {
            _logger = logger;
            _funcTaxCalculateStrategy = funcTaxCalculateStrategy;
            _taxCalculateWithFuncHandler = taxCalculateWithFuncHandler;
            _taxCalculateStrategies = taxCalculateStrategies;
        }

        [HttpGet("taxcalculate/nfe/strategy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        public ActionResult<double> TaxCalculateNfeStrategy([FromQuery] double value)
        {
            var handler = new TaxCalculateHandler(_funcTaxCalculateStrategy(ProductType.Nfe));

            var result = handler.CalculateTax(value);

            LogInformation(result);

            return Ok(result);
        }

        [HttpGet("taxcalculate/nfce/strategy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        public ActionResult<double> TaxCalculateNfceStrategy([FromQuery] double value)
        {
            var handler = new TaxCalculateHandler(_funcTaxCalculateStrategy(ProductType.Nfce));

            var result = handler.CalculateTax(value);

            LogInformation(result);

            return Ok(result);
        }

        [HttpGet("taxcalculate/nfe/strategywithfunc")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        public ActionResult<double> TaxCalculateNfeStrategyWithFunc([FromQuery] double value)
        {
            var result = _taxCalculateWithFuncHandler.CalculateTax(ProductType.Nfe, value);

            LogInformation(result);

            return Ok(result);
        }

        [HttpGet("taxcalculate/nfce/strategywithfunc")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        public ActionResult<double> TaxCalculateNfceStrategyWithFunc([FromQuery] double value)
        {
            var result = _taxCalculateWithFuncHandler.CalculateTax(ProductType.Nfce, value);

            LogInformation(result);

            return Ok(result);
        }

        [HttpGet("taxcalculate/nfe/strategywithenumerable")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        public ActionResult<double> TaxCalculateNfeStrategyWithEnumerable([FromQuery] double value)
        {
            var handler = _taxCalculateStrategies.FirstOrDefault(x => x.ProductType == ProductType.Nfe);

            var result = handler.CalculateTax(value);

            LogInformation(result);

            return Ok(result);
        }

        [HttpGet("taxcalculate/nfce/strategywithenumerable")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        public ActionResult<double> TaxCalculateNfceStrategyWithEnumerable([FromQuery] double value)
        {
            var handler = _taxCalculateStrategies.FirstOrDefault(x => x.ProductType == ProductType.Nfce);

            var result = handler.CalculateTax(value);

            LogInformation(result);

            return Ok(result);
        }
    }
}
