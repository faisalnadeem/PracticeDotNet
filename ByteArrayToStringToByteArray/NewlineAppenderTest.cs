using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ByteArrayToStringToByteArray
{
    public class NewlineAppenderTest
    {

        private const string NEW_LINE_APPENDER = ",\r\n";
        private const string CONTENT_TYPE = "application/json";

        public async Task Test()
        {
            var message = new InvalidRegisterShipmentWebhookMessage()
            {
                CustomerId = "cs_123",
                Errors = new[]{"error1", "error2"},
                RequestBody = "request body ",
                Timestamp = DateTimeOffset.Now
            };

            var invalidRegisterShipmentJsonString = ConvertToJsonString(message);
            await using var fileContent = GenerateStreamFromString($"{invalidRegisterShipmentJsonString}{NEW_LINE_APPENDER}");
            var reader = new StreamReader(fileContent);
            var text = await reader.ReadToEndAsync();

            Console.WriteLine(text);
        }

        private static Stream GenerateStreamFromString(string jsonString)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(jsonString);
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }


        private static string ConvertToJsonString(InvalidRegisterShipmentWebhookMessage invalidRegisterShipmentWebhookMessage)
        {
            return JsonConvert.SerializeObject(invalidRegisterShipmentWebhookMessage, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
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
                });
        }

    }

    public class InvalidRegisterShipmentWebhookMessage
    {
        public string CustomerId { get; set; }
        public string[] Errors { get; set; }
        public string RequestBody { get; set; }
        public DateTimeOffset Timestamp { get; set; }

    }
}
