using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CustomJsonConverterInvalidObject
{
    public class Product
    {
        public string Retailer { get; set; }
        public bool SimulatedTracking { get; set; }
        public string ShippingLocationReference { get; set; }
        public string TenantReference { get; set; }

        public string ChannelReference { get; set; }

        [JsonConverter(typeof(AllowInvalidShipmentTypeJsonConverter))]
        public ShipmentType ShipmentType { get; set; }
    }

    public class AllowInvalidShipmentTypeJsonConverter : JsonConverter
    {
        private static readonly Type ShipmentType = typeof(ShipmentType);

        public override bool CanConvert(Type objectType)
        {
            return objectType == ShipmentType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                //get our globally setuped converter
                var stringEnumConverter = serializer.Converters.FirstOrDefault(x => x.GetType() == typeof(StringEnumConverter)) as StringEnumConverter;

                return stringEnumConverter?.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (Exception)
            {
                //if value is not valid and could not be parsed - return null
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
            // do nothing or RegisterShipmentFromFileMessageTests gets upset when serializing onto a service bus
        }
    }
}