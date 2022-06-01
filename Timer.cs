using System.IO;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace RaFilDaAPI
{
    public class Timer
    {
        public static IScheduler scheduler { get; set; }
        public async Task SetUp()
        {
            if (scheduler != null)
            {
                await scheduler.Shutdown();
                await scheduler.Clear();   
            }
            scheduler = await new StdSchedulerFactory().GetScheduler();
            await Prepare();
        }
        private async Task Prepare()
        {
            IJobDetail job = JobBuilder.Create<MailJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(File.ReadAllText("mailcron.txt")).Build();
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}