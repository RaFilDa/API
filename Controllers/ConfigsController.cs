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

        [HttpGet]
        [Route("GetConfigsByGroupID")]
        public IQueryable<ConfGroup> GetConfigs_ByGroupID(int groupId)
        {
            return myContext.ConfGroups.FromSqlRaw("select * from ConfGroups where GroupID = {0}", groupId);
        }

        [HttpPut]
        [Route("UpdateConfig")]
        public async Task<ActionResult<List<Config>>> UpdateConfig(Config config, int id)
        {
            var dbConf = await myContext.Configs.FindAsync(id);
            if (dbConf == null)
                return NotFound();
            
            dbConf.Name = config.Name;
            dbConf.UserID = config.UserID;
            dbConf.RetentionSize = config.RetentionSize;
            dbConf.BackupFrequency = config.BackupFrequency;
            dbConf.Cron = config.Cron;
            dbConf.TimeZone = config.TimeZone;
            dbConf.PackageSize = config.PackageSize;
            dbConf.BackupType = config.BackupType;
            dbConf.FileType = config.FileType;

            await myContext.SaveChangesAsync();

            return Ok(await myContext.Configs.ToListAsync());
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

        [HttpDelete]
        [Route("RemoveConfigFromComputer")]
        public ActionResult<IQueryable<CompConf>> RemoveComputerFromConfig(int compID, int configID)
        {
            if(compID == 0 || configID == 0)
                return BadRequest();         

            try {
            var deleted =
                myContext.CompConfs.FromSqlRaw("select * from CompConfs where ConfigID = {0} and CompID = {1}",
                    configID, compID).First();
                myContext.CompConfs.Remove(deleted);
            }
            catch { return NotFound(); }
            myContext.SaveChanges();

            return Ok(myContext.CompConfs.FromSqlRaw("select * from CompConfs"));
        }
    }
}