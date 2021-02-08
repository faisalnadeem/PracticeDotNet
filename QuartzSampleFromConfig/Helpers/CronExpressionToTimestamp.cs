using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrontab;
using Topshelf.Configurators;

namespace QuartzSampleFromConfig.Helpers
{
    public class CronExpressionToTimestamp
    {
        IDictionary<string, string> cronExpressionsWithMessage = new Dictionary<string, string>();
        public void TestCronExpressionConversion()
        {
            //ref:http://www.raboof.com/projects/ncrontab/

            cronExpressionsWithMessage.Add("* * * * *", "Every minute.");
            cronExpressionsWithMessage.Add("5 * * * *", "Once every hour and at the fifth minute of the hour(00:05, 01:05, 02:05 etc.).");
            cronExpressionsWithMessage.Add("* 12 * * Mon", "Every minute during the 12th hour of Monday.");
            cronExpressionsWithMessage.Add("* 12 16 * Mon", "Every minute during the 12th hour of Monday, 16th, but only if the day is the 16th of the month.");
            cronExpressionsWithMessage.Add("59 11 * * 1,2,3,4,5", "At 11:59AM on Monday, Tuesday, Wednesday, Thursday and Friday.Every sub-pattern can contain two or more comma separated values.");
            cronExpressionsWithMessage.Add("59 11 * * 1-5", "Equivalent to the previous one.Value ranges are admitted and defined using the minus character.");
            cronExpressionsWithMessage.Add("*/15 9-17 * * *", "Every 15 minutes between the 9th and 17th hour of the day(9:00, 9:15, 9:30, 9:45 and so on... note that the last execution will be at 17:45). The slash character can be used to identify periodic values, in the form of a/ b. A sub - pattern with the slash character is satisfied when the value on the left divided by the one on the right gives an integer result(a % b == 0).");
            cronExpressionsWithMessage.Add("* 12 10-16/2 * *", "Every minute during the 12th hour of the day, but only if the day is the 10th, the 12th, the 14th or the16th of the month.");
            cronExpressionsWithMessage.Add("* 12 1-15,17,20-25 * *", "This pattern causes a task to be launched every minute during the 12th hour of the day, but the day of the month must be between the 1st and the 15th, the 20th and the 25, or at least it must be the 17th.");

            var cronExpressionToTimestamp = new CronExpressionToTimestamp();
            foreach (var crm in cronExpressionsWithMessage)
            {
                Console.WriteLine(crm.Key);
                Console.WriteLine(crm.Value);
                var nextTimestamp = cronExpressionToTimestamp.ConvertCronExpressionToDateTimestamp(crm.Key);
                Console.WriteLine($"{DateTime.UtcNow}: Next timestamp calculated for {crm.Key} is {nextTimestamp}");
                Console.WriteLine("=========================================================================================================\n\n");
            }
        }

        private DateTime? ConvertCronExpressionToDateTimestamp(string cronExpression)
        {
            try
            {
                var schedule = CrontabSchedule.Parse(cronExpression);
                return schedule.GetNextOccurrence(DateTime.Now);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
                return null;
            }
        }
    }
}
