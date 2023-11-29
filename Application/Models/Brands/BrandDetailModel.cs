using Application.Common.Models;

namespace Application.Models.Brands;

public class BrandDetailModel : AuditModel
{
    public string Name { get; set; }
    public string Code { get; set; }
}