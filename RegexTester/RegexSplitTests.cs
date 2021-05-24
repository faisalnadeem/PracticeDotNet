using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace RegexTester
{
    public class RegexSplitTests
    {
        //private const string ADDITIONAL_INFORMATION_REGEX_PATTERN = @"\(([^)]*)\)";


        //private const Regex DATE_FORMAT_PATTERN = new Regex(@"[\b]([\d]{ 2}-[\d]{2}-[\d]{4})");

        Regex dateRegex = new Regex(@"([\d]{2}-[\d]{2}-[\d]{4}).*?([\d]{2}-[\d]{2}-[\d]{4})");
        public const string EDD_START_WITH_REGEX_PATTERN = @"([\d]{2}-[\d]{2}-[\d]{4}).*?([\d]{2}-[\d]{2}-[\d]{4})";

        private const string ADDITIONAL_INFORMATION_START_WITH_REGEX_PATTERN = "^(?:Received into|Returning to|Returned to|Despatched from|Delivered to|Collection attempted, please call|Parcel problem, please call|Parcel damaged, please call)(.*[^Retailer])$";
            //@"^(?:[Received into|Returning to|Returned to|Despatched from|Delivered to|Collection attempted, please call|Parcel problem, please call|Parcel damaged, please call])(.*[^Retailer])$";
        private const string LOCATION_START_WITH_REGEX_PATTERN = "^(?:Received into|Returning to|Returned to|Despatched from|Delivered to)(.*[^Retailer])$";
        private const string PHONE_START_WITH_REGEX_PATTERN = "^(?:Collection attempted, please call|Parcel problem, please call|Parcel damaged, please call)";

        public void TestMatchDatePattern()
        {
            var dateFrom = string.Empty;
            var dateTo = string.Empty;

            var input = "From 23-02-2021 toasdfasdfasd 24-02-2021";

            var match = dateRegex.Match(input);

            if (match.Success)
            {
                dateFrom = match.Groups[1].Value;
                dateTo = match.Groups[2].Value;
            }

            match = Regex.Match(input,
                EDD_START_WITH_REGEX_PATTERN);

            if (match.Success)
            {
                dateFrom = match.Groups[1].Value;
                dateTo = match.Groups[2].Value;
            }


        }
        public void TestMatchPatterns()
        {
            var carrierComments = new List<string>()
            {
                //"Collection on Hold",
                //"Collection problem, we are managing",
                //"Collection delayed, vehicle breakdown",
                //"Collection attempted, we will reschedule",
                "Received into Halifax Delivery Center",
                "Parcel problem, please call(phone number)",
                "Parcel damaged, please call(phone number)",
                "Returned to(location name)",
                "Returned to Retailer",
                "Collection attempted, please call(phone number)",
                "Processed at Central Depot",
                "Received into(location name)",
                "Parcel Misrouted, being directed",
                "Returning to(location name)",
                "Delivery on Hold",
                "Delivery delayed due to bad weather",
                "Address amended, we will reschedule delivery",
                "Loaded into vehicle(van location name)",
                "Despatched from(location name)",
                "Delivery delayed, vehicle broken down",
                "Unable to deliver, to be rescheduled",
                "Delivery attempted, depot to re - book",
                "Delivery declined, returning to sender",
                "Delivered to(location name)",
                "Delivered with signature obtained",
            };

            foreach (var comment in carrierComments)
            {
                //var result = CheckForPhoneNumber(comment);

                if (HasAdditionalValue(comment))
                {
                    var additionalValue = ExtractAdditionalInformation(comment);
                }

            }
        }

        private static string ExtractAdditionalInformation(string carrierDescription)
        {
            //var result = Regex.Split(carrierDescription, ADDITIONAL_INFORMATION_START_WITH_REGEX_PATTERN, RegexOptions.IgnoreCase);
            //return result[1];

            var match = Regex.Match(carrierDescription, ADDITIONAL_INFORMATION_START_WITH_REGEX_PATTERN,
                RegexOptions.IgnoreCase);

            return match.Success ? match.Groups[1].Value : string.Empty;
            //var match = Regex.Match(carrierDescription, ADDITIONAL_INFORMATION_START_WITH_REGEX_PATTERN);
            //return !match.Success ? match.Groups[1].Value : string.Empty;
        }

        private static bool HasAdditionalValue(string carrierDescription)
        {
            return Regex.Match(carrierDescription, ADDITIONAL_INFORMATION_START_WITH_REGEX_PATTERN).Success;
        }
        private Tuple<string, string> CheckForPhoneNumber(string input)
        {
            var fieldName = GetFieldName(input);
            if (string.IsNullOrEmpty(fieldName)) return new Tuple<string, string>("", "");

            var fieldValue = "";
            var match = Regex.Match(input, PHONE_START_WITH_REGEX_PATTERN);

            if (!match.Success) return new Tuple<string, string>(fieldName, fieldValue);

            fieldValue = match.Groups[1].Value;
            return new Tuple<string, string>(fieldName, fieldValue);
        }

        private string GetFieldName(string input)
        {
            var match = Regex.Match(input, LOCATION_START_WITH_REGEX_PATTERN);
            if(match.Success)
            {
                return "Location";
            }

            match = Regex.Match(input, PHONE_START_WITH_REGEX_PATTERN);

            if (match.Success)
            {
                return "Phone";
            }

            return "";
        }


        public void TestSplit()
        {
            var pattern = "[a-z]+|[ (]|[)]";
            var input = "Collection attempted, please call (03331234567)";
            var result = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

            var phoneNumber = result.FirstOrDefault(x => x.StartsWith("(") || x.StartsWith(" ("));

            pattern = "(-)";
            input = "Collection attempted, please call - (03331234567)";
            result = Regex.Split(input, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

            pattern = "[(]|[)]";
            input = "Collection attempted, please call (03331234567)";
            result = Regex.Split(input, pattern, RegexOptions.IgnoreCase);
            
            var regex = new Regex(pattern);
            var match = regex.Match(input);

            regex = new Regex("<key>LibID</key><val>([a-fA-F0-9]{4})</val>");
            match = regex.Match("Before<key>LibID</key><val>A67A</val>After");

            if (match.Success)
            {
                Console.WriteLine("Found Match for {0}", match.Value);
                Console.WriteLine("ID was {0}", match.Groups[1].Value);
            }

            regex = new Regex("<key>LibID</key>[(]");
            match = regex.Match("Before<key>LibID</key>(A67A)");
            if (match.Success)
            {
                Console.WriteLine("Found Match for {0}", match.Value);
                Console.WriteLine("ID was {0}", match.Groups[1].Value);
            }

            regex = new Regex("(" + ")");
            match = regex.Match(input);

            pattern = @"\(([^)]*)\)";
            match = Regex.Match(input, pattern);

            if (match.Success)
            {
                var value1 = match.Groups[0].Value;
                var value2 = match.Groups[1].Value;
            }

        }
    }
}
