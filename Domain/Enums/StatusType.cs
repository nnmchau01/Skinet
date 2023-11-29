using System.ComponentModel;

namespace Domain.Enums;

public enum StatusType
{
    [Description("Order")] Order,
    [Description("Product")] Product,
    [Description("Payment")] Payment,
}