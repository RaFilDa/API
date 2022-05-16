using System.Threading.Tasks;
using Quartz;

namespace RaFilDaAPI
{
    [DisallowConcurrentExecution]
    public class MailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}