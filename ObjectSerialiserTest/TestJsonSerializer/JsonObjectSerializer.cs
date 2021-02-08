using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ObjectSerialiserTest.TestJsonSerializer
{
    public static class JsonObjectSerializer
    {
        const string JSON_MEDIA_TYPE = "application/json";
        private static readonly Random _random = new Random();
        private static Fixture _fixture = new Fixture();

        public static void MainTest()
        {
            try
            {
                var shipment = new Fixture().Build<Shipment>().Create();
                var stringContent =  GetStringContentFromObject(shipment);

                var shipment1 = _fixture.Build<Shipment>()
                    .With(s => s.TrackingReferences, GetRandomStringArray(1, 20, 3, 50))
                    .With(x => x.CustomReferences, GetRandomStringArray(1, 20, 3, 50))
                    .With(x => x.Tags, GetRandomStringArray(1, 20, 3, 30))
                    .With(x => x.PackageTrackingReference, RandomString(10, 20))
                    .Create();
                var stringContent1 =  GetStringContentFromObject(shipment1);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string RandomString(int from, int? to = null)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, _random.Next(from, to ?? from)).Select(s => s[_random.Next(s.Length)]).ToArray());
        }


        private static string[] GetRandomStringArray(int countFrom, int countTo, int lenFrom, int lenTo)
        {
            return _fixture.Build<string>()
                .FromSeed(x => RandomString(lenFrom, lenTo))
                .CreateMany(_random.Next(countFrom, countTo))
                .ToArray();
        }


        private static StringContent GetStringContentFromObject(Shipment shipment)
        {
                var snakeCased = shipment.SerializeToSnakeCase();
                var content = GetStringContent(snakeCased);
                return content;
        }

        public static StringContent GetStringContent(this string value)
        {
            return value != null ? new StringContent(value, Encoding.UTF8, JSON_MEDIA_TYPE) : null;
        }

        public static string SerializeToSnakeCase(this object value)
        {
            try
            {
                var abc = value != null ? JsonConvert.SerializeObject(value, SnakeSerializerSettings) : null;
                return abc;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static readonly JsonSerializerSettings SnakeSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter
                {
                    AllowIntegerValues = false
                }
            },
        };
    }
}
