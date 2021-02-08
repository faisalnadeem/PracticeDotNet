using System.Runtime.Serialization;

namespace CustomJsonConverterInvalidObject
{
    public enum ShipmentType
    {
        /// <summary>
        /// A shipment being delivered to a customer�s address
        /// </summary>
        [EnumMember(Value = "delivery")]
        Delivery,
        /// <summary>
        /// A shipment being returned to the warehouse, sender, retailer or manufacturer
        /// </summary>
        [EnumMember(Value = "return")]
        Return,
        /// <summary>
        /// A shipment that the consumer will pick up
        /// </summary>
        [EnumMember(Value = "pick_up")]
        PickUp,
        /// <summary>
        /// A shipment that is dropped off for delivery
        /// </summary>
        [EnumMember(Value = "drop_off")]
        DropOff,
        /// <summary>
        /// A shipment that is dropped off for delivery for the consumer to pick-up
        /// </summary>
        [EnumMember(Value = "drop_off_pick_up")]
        DropOffPickUp,
        /// <summary>
        /// A shipment that is dropped off for return
        /// </summary>
        [EnumMember(Value = "return_drop_off")]
        ReturnDropOff,
    }
}