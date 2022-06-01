using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;
using Quartz;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Reports")]
    public class ReportsController : ControllerBase
    {
        private readonly MyContext myContext;

        public ReportsController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        [Authorize(Role = "admin")]
        public async Task<ActionResult<List<ReportDetail>>> GetReports()
        {
            return Ok(myContext.ReportDetails.FromSqlRaw("select r.id, r.date, cf.Name, cp.MAC, r.Type as backup, r.IsError as state, r.Message from Reports r inner join CompConfs cc on cc.id = r.CompConfID inner join Computers cp on cp.ID = cc.CompID inner join Configs cf on cf.id = cc.ConfigID order by r.Date desc"));
        }

        [HttpPost("/cron")]
        [Authorize(Role = "admin")]
        public async Task<ActionResult> UpdateCron(string cron)
        {
            System.IO.File.WriteAllText("mailCron.txt", cron);
            
            await Timer.scheduler.Clear();
            IJobDetail job = JobBuilder.Create<MailJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cron).Build();
            await Timer.scheduler.ScheduleJob(job, trigger);
            await Timer.scheduler.Start();
            
            return Ok();
        }
        
        [HttpGet("/cron")]
        [Authorize(Role = "admin")]
        public JsonResult GetCron()
        {
            return new JsonResult(System.IO.File.ReadAllText("mailcron.txt"));
        }

        [HttpPost("/mailsettings")]
        [Authorize(Role = "admin")]
        public async Task<ActionResult> UpdateMailSettings(string[] settings)
        {
            var fileArr = System.IO.File.ReadAllLines("mailInfo.txt").Take(1);
            fileArr = fileArr.Concat(settings);
            System.IO.File.WriteAllLines("mailInfo.txt", fileArr);
            return Ok();
        }
        
        [HttpGet("/mailsettings")]
        [Authorize(Role = "admin")]
        public async Task<ActionResult<string[]>> GetMailSettings()
        {
            return Ok(System.IO.File.ReadAllLines("mailInfo.txt").Skip(1).ToArray());
        }

        [HttpPost]
        [Authorize(Role = "admin,daemon")]
        public async Task<ActionResult<List<Report>>> AddReport(Report report)
        {
            myContext.Reports.Add(report);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Reports.ToListAsync());
        }

        [HttpDelete("{id}")]
        [Authorize(Role = "admin")]
        public async Task<ActionResult<List<Report>>> DeleteReport(int id)
        {
            var rep = await myContext.Reports.FindAsync(id);
            if (rep == null)
                return NotFound();

            myContext.Reports.Remove(rep);
            myContext.SaveChanges();

            return Ok(await myContext.Reports.ToListAsync());
        }

        /*[HttpDelete("{date}")]
        public async Task<ActionResult<List<Report>>> DeleteReportByDate(DateTime date)
        {
            //TBA
        } */
    }
}