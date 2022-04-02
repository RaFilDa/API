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
    [Route("Reports")]
    public class ReportsController : ControllerBase
    {
        private readonly MyContext myContext;

        public ReportsController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Report>>> GetReports()
        {
            return Ok(await myContext.Reports.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Report>>> AddReport(Report report)
        {
            myContext.Reports.Add(report);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Reports.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Report>>> DeleteReport(int id)
        {
            var rep = await myContext.Reports.FindAsync(id);
            if (rep == null)
                return NotFound();

            myContext.Reports.Remove(rep);
            myContext.SaveChanges();

            return Ok(await myContext.Reports.ToListAsync());
        }
    }
}