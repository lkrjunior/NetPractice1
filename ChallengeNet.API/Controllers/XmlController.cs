using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChallengeNet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XmlController : ControllerBase
    {
        private readonly ILogger<XmlController> _logger;
        private readonly IGenerateXmlHandler _generateXmlHandler;

        public XmlController(ILogger<XmlController> logger, IGenerateXmlHandler generateXmlHandler)
        {
            _logger = logger;
            _generateXmlHandler = generateXmlHandler;
        }

        [HttpGet("generatexml/nfe/strategyattribute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GenerateXmlNfeStrategyAttribute()
        {
            var xml = _generateXmlHandler.GenerateXml(ProductType.Nfe);

            return Ok(xml.ToString());
        }

        [HttpGet("generatexml/nfce/strategyattribute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GenerateXmlNfceStrategyAttribute()
        {
            var xml = _generateXmlHandler.GenerateXml(ProductType.Nfce);

            return Ok(xml.ToString());
        }
    }
}