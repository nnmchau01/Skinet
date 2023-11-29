using Application.Common.Models;

namespace Application.Models.Promotion;

public class PromotionDetailModel : AuditModel
{
    public string Code { get; set; }
    public int DiscountPercent { set; get; }
}