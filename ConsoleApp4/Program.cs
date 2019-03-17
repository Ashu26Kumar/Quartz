using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class Program
    {
         public static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }
        public static async Task Run()
        {
            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();
                await scheduler.Start();

                IJobDetail job = JobBuilder.Create<SimpleJob>().WithIdentity("Job1","Group1").Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("Trigger1", "Group1").StartNow().WithSimpleSchedule(x=>x.WithIntervalInSeconds(1).WithRepeatCount(10)).Build();
                await scheduler.ScheduleJob(job, trigger);
                await Task.Delay(TimeSpan.FromSeconds(10));
                await Console.Out.WriteLineAsync("Press any key ");
                await Console.In.ReadLineAsync();
            }
            catch (SchedulerException e)
            {
                await Console.Error.WriteLineAsync(e.ToString());
            }
        }
    }
}
