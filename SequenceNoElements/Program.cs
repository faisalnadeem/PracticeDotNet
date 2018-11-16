using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SequenceNoElements
{
    class Program
    {

        static void Main(string[] args)
        {
            //CurrentActivityTests.TestRegisterationProgressMatch();
            CurrentActivityTests.TestJsonRequest();
            Console.ReadKey();
        }
    }

    public class CurrentActivityTests
    {

        private static List<string> _activities;

        public static void TestRegisterationProgressMatch()
        {
            foreach (var activity in CurrentActivity.PublicActivities)
            {
                try
                {
                    var failureStep = RegistrationProgress.GetIdvErrorByDescription(activity.Description);
                    Console.WriteLine($"Successfully matched {activity.Description} => {failureStep.Value}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"XXXXXXX - No match for {activity.Description}");
                    //Console.WriteLine(e);
                    //throw;
                }
            }

        }

        public static void TestJsonRequest()
        {
            var request = new IdvRequest() {CurrentActivity = ""};
            var currentActivity1 = CurrentActivity.GetByDescription(request.CurrentActivity?.Trim());
            LoadJson();
            //Console.WriteLine(currentActivity);

            var _matchingItems = new List<string>();
            var _notMatchingItems = new List<string>();

            foreach (var activity in _activities)
            {
                Console.WriteLine("Performing activity found check form => " + activity);
                var currentActivity = CurrentActivity.GetByDescription(activity.Trim());
                if (currentActivity == CurrentActivity.NoActivity)
                {
                    _notMatchingItems.Add(activity);
                    Console.WriteLine("No current activity found from the sequence");
                }
                else
                {
                    _matchingItems.Add(activity);
                    Console.WriteLine(
                        $"For activity \"{activity}\" found matching current activity \"{currentActivity.Description}\"");

                }

            }

            File.WriteAllLines(@"C:\sftptemp\MatchingActivities.txt", _matchingItems);
            File.WriteAllLines(@"C:\sftptemp\NotMatchingActivities.txt", _notMatchingItems);

        }

        public static void LoadJson()
        {
            _activities = new List<string>();
            using (StreamReader r = new StreamReader("workflowWrapper.json"))
            {
                string json = r.ReadToEnd();
                var workflowJson = JsonConvert.DeserializeObject<WorkflowJson>(json);
                foreach (var activity in workflowJson.Workflow.Activities)
                {
                    _activities.Add(activity.Name);
                    Console.WriteLine("Activity from CV => " + activity.Name);
                }
            }
        }
    }


    public sealed class CurrentActivity
    {
        private CurrentActivity(string description)
        {
            Description = description;
        }

        public string Description { get; }

        // ReSharper disable InconsistentNaming
        public static CurrentActivity DataCollection { get; } = new CurrentActivity("Data Collection");
        public static CurrentActivity Matching { get; } = new CurrentActivity("Matching");
        public static CurrentActivity MatchingCheck { get; } = new CurrentActivity("Matching Check");
        public static CurrentActivity PredictiveAnalytics { get; } = new CurrentActivity("Predictive Analytics");

        public static CurrentActivity PredictiveAnalyticsCheck { get; } =
            new CurrentActivity("Predictive Analytics Check");

        public static CurrentActivity IdEnchanced { get; } = new CurrentActivity("Id Enhanced");
        public static CurrentActivity IdEnchancedCheck { get; } = new CurrentActivity("Id Enchanced Check");
        public static CurrentActivity RTFA { get; } = new CurrentActivity("RTFA");
        public static CurrentActivity RTFACheck { get; } = new CurrentActivity("RTFA Check");
        public static CurrentActivity GetKBAQuestions { get; } = new CurrentActivity("Get KBA Questions");

        public static CurrentActivity EnoughQuestionsRound1 { get; } =
            new CurrentActivity("Enough Questions Round 1");

        public static CurrentActivity KBA1 { get; } = new CurrentActivity("KBA 1");
        public static CurrentActivity KBA1Check { get; } = new CurrentActivity("KBA 1 Check");

        public static CurrentActivity EnoughQuestionsRound2 { get; } =
            new CurrentActivity("Enough Questions Round 2");

        public static CurrentActivity KBA2 { get; } = new CurrentActivity("KBA 2");
        public static CurrentActivity KBA2Check { get; } = new CurrentActivity("KBA 2 Check");
        public static CurrentActivity DeviceRiskCheck { get; } = new CurrentActivity("Device Risk Check");
        public static CurrentActivity EmailRisk { get; } = new CurrentActivity("Email Risk");
        public static CurrentActivity EmailRiskCheck { get; } = new CurrentActivity("Email Risk Check");
        public static CurrentActivity MobileRisk { get; } = new CurrentActivity("Mobile Risk");
        public static CurrentActivity MobileRiskCheck { get; } = new CurrentActivity("Mobile Risk Check");

        public static CurrentActivity NoActivity { get; } = new CurrentActivity(null);
        // ReSharper restore InconsistentNaming

        private static readonly CurrentActivity[] ACTIVITIES =
        {
            DataCollection,
            Matching,
            MatchingCheck,
            PredictiveAnalytics,
            PredictiveAnalyticsCheck,
            IdEnchanced,
            IdEnchancedCheck,
            RTFA,
            RTFACheck,
            GetKBAQuestions,
            EnoughQuestionsRound1,
            KBA1,
            KBA1Check,
            EnoughQuestionsRound2,
            KBA2,
            KBA2Check,
            DeviceRiskCheck,
            EmailRisk,
            EmailRiskCheck,
            MobileRisk,
            MobileRiskCheck,
            NoActivity
        };

        public static CurrentActivity GetByDescription(string description)
        {
            //var currentActivity = ACTIVITIES.FirstOrDefault(x =>
            //    string.Equals(x.Description, description, StringComparison.OrdinalIgnoreCase));

            //return currentActivity ?? NoActivity;

            return ACTIVITIES.First(x =>
                string.Equals(x.Description, description, StringComparison.OrdinalIgnoreCase));
        }

        public static CurrentActivity[] PublicActivities => ACTIVITIES;
    }

    public class IdvRequest

    {
        public string CurrentActivity { get; set; }
    }
}
