using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CustomJsonConverterInvalidObject
{
    class Program
    {
        static void Main(string[] args)
        {
            new JsonConverterTest().Test();
            Console.WriteLine("Hello World!");
        }
    }

    public class JsonConverterTest
    {
        public void Test()
        {
            var product = new Fixture().Build<Product>().With(x => x.ShipmentType, ShipmentType.DropOffPickUp).Create();
            //product.ShipmentType = ShipmentType.Delivery;
            var jsonString = TrySerializing(product);

            var desProduct = TryDeSerializing(jsonString);

        }

        private Product TryDeSerializing(string jsonString)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter
                {
                    AllowIntegerValues = false
                }
            );
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy
                {
                    OverrideSpecifiedNames = true
                }
            };


            var jsonObject = JsonConvert.DeserializeObject<Product>(jsonString, settings);
            return jsonObject;

        }

        private string TrySerializing(Product message)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter
                {
                    AllowIntegerValues = false
                }
            );
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy
                {
                    OverrideSpecifiedNames = true
                }
            };

            string jsonString = JsonConvert.SerializeObject(message, settings);
            return jsonString;
            //var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            //var msg = new Message(jsonBytes);
            //return msg;
        }
    }
}
