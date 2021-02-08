using System;
using Newtonsoft.Json;

namespace ObjectSerialiserTest.TestJsonSerializer
{
    public class Shipment
    {
        /// <summary>
        /// The tracking reference(s) for this shipment
        /// Validation rules: Length &gt;= 3Length &lt;= 50
        /// Occurence rules: 1..1
        /// </summary>
        public string[] TrackingReferences { get; set; }
        /// <summary>
        /// The tracking reference for a package that belongs to this shipment.
        /// Validation rules: Length &gt;= 3 Length &lt;= 50
        /// Occurence rules: 1..1
        /// </summary>
        public string PackageTrackingReference { get; set; }
        /// <summary>
        /// Your own reference(s) for this shipment
        /// Validation rules: Length &gt;= 3Length &lt;= 50
        /// Occurence rules: 0..20
        /// </summary>
        public string[] CustomReferences { get; set; }
        /// <summary>
        /// Optional tags for this shipment.
        /// Validation rules: Length &gt;= 1Length &lt;= 30
        /// Occurence rules: 0..20
        /// </summary>
        public string[] Tags { get; set; }
        /// <summary>
        /// The name of the carrier for this shipment
        /// Validation rules: Length &lt;= 100
        /// Occurence rules: 0..1
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Carrier reference for this shipment
        /// Validation rules: 
        /// Occurence rules: 0..1
        /// </summary>
        public string CarrierReference { get; set; }
        /// <summary>
        /// The name of the carrier service for this shipment
        /// Validation rules: Length &lt;= 100
        /// Occurence rules: 0..1
        /// </summary>
        public string CarrierService { get; set; }
        /// <summary>
        /// The date on which the shipment was (or will be) shipped.
        /// Validation rules: Must be in the format YYYY-MM-DDThh:mm:ss+|-hh:mm[1].
        /// Occurence rules: 0..1
        /// </summary>
        public DateTimeOffset? ShippedDate { get; set; }
        /// <summary>
        /// The date on which the order was placed
        /// Validation rules: Must be in the format YYYY-MM-DDThh:mm:ss+|-hh:mm1
        /// Occurence rules: 0..1
        /// </summary>
        public DateTimeOffset? OrderDate { get; set; }
        /// <summary>
        /// An identifier for the retailer for this shipment
        /// Validation rules: Length &lt;= 100
        /// Occurence rules: 0..1
        /// </summary>
        public string Retailer { get; set; }

        public string ShippingLocationReference { get; set; }

        public string TenantReference { get; set; }

        public string ChannelReference { get; set; }
    }
}