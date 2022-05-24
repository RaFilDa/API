using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Quartz.Impl;
using System.Net.Http.Headers;

namespace RaFilDaAPI
{
    public static class Scheduler
    {
        public static IScheduler _scheduler = null;
        
        public static void BuildScheduler()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler = scheduler;
            CreateJob();
        }
        
        public static void CreateJob()
        {
            if (_scheduler != null)
            {
                _scheduler.Shutdown();
                _scheduler.Clear();
            }

            string cron = File.ReadAllText(@".\mailCron.txt");
            var jobKey = new JobKey("MailJob");
            IJobDetail job = JobBuilder.Create<MailJob>()
                .WithIdentity(jobKey)
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .WithIdentity("t_MailJob")
                .WithCronSchedule(cron)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
            _scheduler.Start();
        } 
    }
}