using System.ComponentModel;

namespace Domain.Enums;

public enum OrderStatus
{
    // Code
    [Description("AwaitingShip")] AwaitingShipCode,
    [Description("Shipping")] ShippingCode,
    [Description("Reject")] RejectCode,
    [Description("Cancel")] CancelCode,
    [Description("Received")] ReceivedCode,

    // Display
    [Description("Chờ gửi hàng")] AwaitingShipDisplayVN,
    [Description("Đang gửi")] ShippingDisplayVN,
    [Description("Từ chối nhận")] RejectDisplayVN,
    [Description("Đã bị hủy")] CancelDisplayVN,
    [Description("Đã nhận hàng")] ReceivedDisplayVN,

    // Display
    [Description("Awaiting Ship")] AwaitingShipDisplayEN,
    [Description("Shipping")] ShippingDisplayEN,
    [Description("Reject")] RejectDisplayEN,
    [Description("Cancel")] CancelDisplayEN,
    [Description("Received")] ReceivedDisplayEN,
}