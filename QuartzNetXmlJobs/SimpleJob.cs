using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetXmlJobs
{
    using Quartz;
    using Common.Logging;

    public class SimpleJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SimpleJob));

        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with
        /// the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            // This job simply prints out its job name and the
            // date and time that it is running
            JobKey jobKey = context.JobDetail.Key;
            log.InfoFormat("Executing job: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));

            if (context.MergedJobDataMap.Count > 0)
            {
                ICollection<string> keys = context.MergedJobDataMap.Keys;
                foreach (string key in keys)
                {
                    String val = context.MergedJobDataMap.GetString(key);
                    log.InfoFormat(" - jobDataMap entry: {0} = {1}", key, val);
                }
            }

            context.Result = "hello";
        }
    }
}
