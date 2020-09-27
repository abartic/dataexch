using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataExch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockInfoController : ControllerBase
    {
        private readonly ILogger<StockInfoController> _logger;

        public StockInfoController(ILogger<StockInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<StockInfo> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(index => 
            {
                var s = new StockInfo()
                {
                    Date = DateTime.Now.AddDays(index),
                    OpenPrice = rng.Next(1, 120),
                    ClosePrice = rng.Next(1, 120),
                };
                s.Summary = s.OpenPrice <= s.ClosePrice ? "Up" : "Down";
                return s;
            })
            .ToArray();
        }
    }
}
