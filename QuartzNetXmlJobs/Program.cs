using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetXmlJobs
{
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Matchers;
    using Quartz.Simpl;
    using Quartz.Spi;
    using Quartz.Xml;
    using Quartz.Xml.JobSchedulingData20;
    using System.IO;
    using System.Xml.Serialization;

    using Common.Logging;
    using System.Collections.Specialized;

    class Program
    {
        public static StdSchedulerFactory SchedulerFactory;
        public static IScheduler Scheduler;

        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program));

            SchedulerFactory = new StdSchedulerFactory();
            Scheduler = SchedulerFactory.GetScheduler();

            Scheduler.Start();

            Console.WriteLine("Quartz.Net Started ...");

            Console.WriteLine("***** FETCHING JOBS *****");

            GetAllJobs(Scheduler);

            Console.WriteLine("******************");

            Console.ReadLine();
        }

       
        private static void GetAllJobs(IScheduler scheduler)
        {
            int countJobs = 0;
            IList<string> jobGroups = scheduler.GetJobGroupNames();
            IList<string> triggerGroups = scheduler.GetTriggerGroupNames();

            foreach (string group in jobGroups)
            {
                countJobs++;

                Console.WriteLine(string.Format("----- JOB #{0} -----", countJobs.ToString()));

                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = scheduler.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var detail = scheduler.GetJobDetail(jobKey);
                    var triggers = scheduler.GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        Console.WriteLine("JOB GROUP: " + group);
                        Console.WriteLine("JOB NAME: " + jobKey.Name);
                        Console.WriteLine("JOB DESCRIPTION: " + detail.Description);
                        Console.WriteLine("TRIGGER NAME: " + trigger.Key.Name);
                        Console.WriteLine("TRIGGER GROUP: " + trigger.Key.Group);
                        Console.WriteLine("TRIGGER TYPE: " + trigger.GetType().Name);
                        Console.WriteLine("TRIGGER STATE: " + scheduler.GetTriggerState(trigger.Key));
                        DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            Console.WriteLine("NEXT FIRE TIME: " + nextFireTime.Value.LocalDateTime.ToString());
                        }

                        DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            Console.WriteLine("PREVIOUS FIRE TIME: " + previousFireTime.Value.LocalDateTime.ToString());
                        }
                    }
                }

                Console.WriteLine(string.Format("/----- JOB #{0} -----/", countJobs.ToString()));
            }
        }
    }
}
