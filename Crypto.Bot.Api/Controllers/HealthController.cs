using Crypto.Bot.Domain.Entity;
using Crypto.Bot.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        // GET api/check
        [HttpGet]
        public ActionResult Check()
        {
            return Ok();
        }
    }
}
