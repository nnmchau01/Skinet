using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Promotion;

public class UpdatePromotionModel : AuditModel
{
    public int DiscountPercent { set; get; }
}