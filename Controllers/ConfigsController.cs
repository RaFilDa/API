using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Configs")]
    public class ConfigsController : ControllerBase
    {
        private readonly MyContext myContext;

        public ConfigsController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Config>>> GetConfigs()
        {
            return Ok(await myContext.Configs.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Config>>> AddConfig(Config config)
        {
            myContext.Configs.Add(config);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Configs.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Config>>> DeleteConfig(int id)
        {
            var config = await myContext.Configs.FindAsync(id);
            if (config == null)
                return NotFound();

            myContext.Configs.Remove(config);
            myContext.SaveChanges();

            return Ok(await myContext.Configs.ToListAsync());
        }
    }
}