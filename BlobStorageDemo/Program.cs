using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml;
using Newtonsoft.Json.Converters;

namespace BlobStorageDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            var randomString = RandomStringGenerator.RandomString(48);
            PasLogicBlobStorageHelper.ListBlobs();
            Console.WriteLine("Hello World!");
        }
    }

    public class RandomStringGenerator
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }


    public class ShipmentTrackingEvent
    {
        public ShipmentSummary Shipment { get; set; }

        public TrackingEvent[] Events { get; set; }
    }
    public class ShipmentSummary
    {
        public string Id { get; set; }


        public bool MayBeMissing { get; set; }

        public string Retailer { get; set; }

        public string CustomerId { get; set; }

        public string Carrier { get; set; }

        public string CarrierReference { get; set; }

        public string PackageTrackingReference { get; set; }

        public string CarrierIntegrationId { get; set; }

        public string[] CustomReferences { get; set; } = { };

        public string[] TrackingReferences { get; set; } = { };

        public string[] Tags { get; set; }

        public DateTimeOffset? DeliveredDate { get; set; }

        public DateTimeOffset? LastTracked { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public bool IsTrackable { get; set; }

        public string[] CarrierTrackingUrls { get; set; }

        public string ShippingLocationReference { get; set; }

        public string TenantReference { get; set; }

        public string ChannelReference { get; set; }
    }
    public class TrackingEvent
    {
        public string Id { get; set; }

        public string CarrierIntegrationId { get; set; }

        public string CarrierId { get; set; }

        public string SortedCarrierName { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset ReceivedTimestamp { get; set; }

        public string EventCode { get; set; }

        public int EventPriority { get; set; }

        public string RawEvent { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string ShipmentId { get; set; }

        public string CustomerId { get; set; }

        public string CorrelationId { get; set; }

        public string EventSignature { get; set; }

        public string TrackingReference { get; set; }

        public string PackageTrackingReference { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

    }
}
