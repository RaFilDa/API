using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace RaFilDaAPI
{
    public class Program
    {
        public static IScheduler _sheduler = null;
        public static void Main(string[] args)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().Result;
            _sheduler = scheduler;
            _sheduler.Start();
            CreateJob();

            /*
            var cron = File.ReadAllText(@".\mailCron.txt");
            ITrigger newTrigger = TriggerBuilder.Create()
                .ForJob("MailJob")
                .WithIdentity("t_MailJob")
                .WithCronSchedule(cron)
                .Build();
            _sheduler.RescheduleJob(new TriggerKey("t_MailJob"), newTrigger); */
        }
        
        public static void CreateJob()
        {
            var cron = File.ReadAllText(@".\mailCron.txt");
            IJobDetail job = JobBuilder.Create<MailJob>()
                .WithIdentity("MailJob")
                .Build();
            
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob("MailJob")
                .WithIdentity("t_MailJob")
                .WithCronSchedule(cron)
                .Build();
        }
    }
}
