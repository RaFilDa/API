using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Records.NotUsed;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Configs")]
    [Authorize(Role = "admin")]
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

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<Config>>>  GetConfig(int Id)
        {
            var conf = await myContext.Configs.FindAsync(Id);
            if (conf == null)
                return NotFound();

            return Ok(conf);
        }
        
        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<int>> GetConfigByName(string name)
        {
            var conf = myContext.Configs.Where(x => x.Name == name).First();
            if (conf == null)
                return NotFound();

            return Ok(conf.Id);
        }

        [HttpGet("Destination/{configId}")]
        public IQueryable<Destination> GetDestinationForConfig(int configId)
        {
            return myContext.Destination.FromSqlRaw("select * from Destination where ConfigID = {0}", configId);
        }

        [HttpGet("Source/{configId}")]
        public IQueryable<Source> GetSourceForConfig(int configId)
        {
            return myContext.Source.FromSqlRaw("select * from Source where ConfigID = {0}", configId);
        }

        [HttpPost]
        [Route("Source")]
        public async Task<ActionResult<List<Source>>> AddSource(Source source)
        {
            myContext.Source.Add(source);
            await myContext.SaveChangesAsync();
            return Ok(await myContext.Source.ToListAsync());
        }
        
        [HttpPost]
        [Route("Destination")]
        public async Task<ActionResult<List<Destination>>> AddDestination(Destination destination)
        {
            myContext.Destination.Add(destination);
            await myContext.SaveChangesAsync();
            return Ok(await myContext.Destination.ToListAsync());
        }

        [HttpGet]
        [Route("GetConfigsByGroupID")]
        public IQueryable<ConfGroup> GetConfigs_ByGroupID(int groupId)
        {
            return myContext.ConfGroups.FromSqlRaw("select * from ConfGroups where GroupID = {0}", groupId);
        }
        
        [HttpGet]
        [Route("GetConfigsByCompID/{id}")]
        public IQueryable<Config> GetConfigs_ByComputer(int id)
        {
            return myContext.Configs.FromSqlRaw("select c.* from CompConfs cc inner join Configs c on c.id = cc.ConfigID where CompID = {0}", id);
        }

        [HttpPut]
        [Route("UpdateConfig")]
        public async Task<ActionResult<List<Config>>> UpdateConfig(Config config, int id)
        {
            var dbConf = await myContext.Configs.FindAsync(id);
            if (dbConf == null)
                return NotFound();
            
            dbConf.Name = config.Name;
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

        [HttpDelete]
        [Route("RemoveSourceFromConfig")]
        public async Task<ActionResult<List<Config>>> RemoveSourceFromConfig(int sourceId)
        {
            var src = await myContext.Source.FindAsync(sourceId);
            if (src == null)
                return NotFound();

            myContext.Source.Remove(src);
            myContext.SaveChanges();

            return Ok(await myContext.Source.ToListAsync());
        }

        [HttpDelete]
        [Route("RemoveDestinationFromConfig")]
        public async Task<ActionResult<List<Destination>>> RemoveDestinationFromConfig(int destId)
        {
            var dest = await myContext.Destination.FindAsync(destId);
            if (dest == null)
                return NotFound();

            myContext.Destination.Remove(dest);
            myContext.SaveChanges();

            return Ok(await myContext.Destination.ToListAsync());
        }
    }
}