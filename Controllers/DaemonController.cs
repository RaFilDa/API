using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Daemon")]

    public class DaemonController : ControllerBase
    {
        private readonly MyContext myContext;

        public DaemonController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet("{ComputerID}")]
        public List<DaemonInfo> GetCron(int ComputerID)
        {
            var daemonList = new List<DaemonInfo>();
            var configList = myContext.Configs.FromSqlRaw(
                "select c.ID, c.Name, c.UserID, c.RetentionSize, c.BackupFrequency, c.Cron, c.TimeZone, c.PackageSize, c.BackupType, c.FileType from Configs c inner join CompConfs cc on c.ID = cc.ConfigID where cc.CompID = {0}",
                ComputerID).ToList();
            foreach (var config in configList)
            {
                var daemon = new DaemonInfo()
                {
                    Cron = config.Cron, Destinations = myContext.Destination.FromSqlRaw("select * from Destination where ConfigId = {0}", config.Id).ToList(), Sources = myContext.Source.FromSqlRaw("select * from Source where ConfigId = {0}", config.Id).ToList()
                };
                daemonList.Add(daemon);
            }

            return daemonList;
        }
    }
}
