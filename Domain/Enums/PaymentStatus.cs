using System.ComponentModel;

namespace Domain.Enums;

public enum PaymentStatus
{
    // Code
    [Description("Paid")] PaidCode,
    [Description("WaitingPay")] WaitingPayCode,

    // Display
    [Description("Đã thanh toán")] PaidDisplayVN,
    [Description("Chưa thanh toán")] WaitingPayDisplayVN,

    // Display
    [Description("Paid")] PaidDisplayEN,
    [Description("Waiting Pay")] WaitingPayDisplayEN,
}