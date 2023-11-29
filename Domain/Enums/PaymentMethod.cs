using System.ComponentModel;

namespace Domain.Enums;

public enum PaymentMethod
{
    // Code
    [Description("VNPay")] VnPayCode,
    [Description("Cash")] CashCode,
    [Description("PayPal")] PayPalCode,

    // Display
    [Description("VNPay")] VnPayDisplayVN,
    [Description("PayPal")] PayPalDisplayVN,
    [Description("Tiền mặt")] CashDisplayVN,

    // Display
    [Description("VNPay")] VnPayDisplayEN,
    [Description("PayPal")] PayPalDisplayEN,
    [Description("Cash")] CashDisplayEN
}