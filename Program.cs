using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace RaFilDaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        var jobKey = new JobKey("MailJob");
                        var cron = File.ReadAllLines(@"..\mailcron.txt");
                        q.AddJob<MailJob>(opts => opts.WithIdentity(jobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity("t_MailJob")
                            .WithCronSchedule(cron[0])
                        );
                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
    }
}
