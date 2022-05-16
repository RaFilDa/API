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

                        var JobKey = new JobKey("MailJob");
                        q.AddJob<MailJob>(opts => opts.WithIdentity(JobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(JobKey)
                            .WithIdentity("t_MailJob")
                            .WithCronSchedule("0 0 0 * * ?")
                        );
                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
    }
}
