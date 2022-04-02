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

        [HttpGet("{computerId}")]
        public IQueryable<CompConf> GetConfigs_ByComputerID(int computerId)
        {
            return myContext.CompConfs.FromSqlRaw("select * from CompConfs where CompID = {0}", computerId);
        }

        [HttpPost]
        public async Task<ActionResult<List<Config>>> AddConfig(Config config)
        {
            myContext.Configs.Add(config);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Configs.ToListAsync());
        }

        [HttpPost]
        [Route("AddConfigToComputer")]
        public ActionResult<IQueryable<CompConf>> AddConfigToComputer(int confId, int compId)
        {
            if(confId == 0 || compId == 0)
                return BadRequest();

            var compConf = new CompConf{
                Id = 0, 
                ConfigID = confId,
                CompID = compId
            };
            myContext.CompConfs.Add(compConf);
            myContext.SaveChanges();

            return Ok(myContext.CompConfs.FromSqlRaw("select * from CompConfs"));
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