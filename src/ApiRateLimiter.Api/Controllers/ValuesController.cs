using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRateLimiter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            string result = "OK";
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetAll(int id)
        {
            return Ok(id);
        }
    }
}
