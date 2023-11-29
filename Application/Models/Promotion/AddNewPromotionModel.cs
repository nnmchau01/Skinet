using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Promotion;

public class AddNewPromotionModel : AuditModel
{
    public string Code { get; set; }
    public int DiscountPercent { set; get; }
}