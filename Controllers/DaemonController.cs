using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Daemon")]
    [Authorize(Role = "admin,daemon")]
    public class DaemonController : ControllerBase
    {
        private readonly MyContext myContext;

        public DaemonController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet("{ComputerID}")]
        public List<DaemonInfo> GetDeamonInfo(int ComputerID)
        {
            var daemonList = new List<DaemonInfo>();
            var configList = myContext.Configs.FromSqlRaw(
                "select c.ID, c.Name, c.RetentionSize, c.BackupFrequency, c.Cron, c.TimeZone, c.PackageSize, c.BackupType, c.FileType from Configs c inner join CompConfs cc on c.ID = cc.ConfigID where cc.CompID = {0}",
                ComputerID).ToList();
            foreach (var config in configList)
            {
                var daemon = new DaemonInfo()
                {
                    Config = config, Destinations = myContext.Destination.FromSqlRaw("select * from Destination where ConfigId = {0}", config.Id).ToList(), Sources = myContext.Source.FromSqlRaw("select * from Source where ConfigId = {0}", config.Id).ToList()
                };
                daemonList.Add(daemon);
            }

            return daemonList;
        }
        
        [HttpGet("GetCompConfByCompID&ConfID")]
        public IQueryable<CompConf> GetCompConfsByCompID_ConfID(int confId, int compId)
        {
            return myContext.CompConfs.FromSqlRaw("select * from CompConfs c where c.ConfigID = {0} AND c.CompID = {1}", confId, compId);
        }
    }
}
