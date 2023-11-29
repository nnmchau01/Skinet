using Application.Common.Models;

namespace Application.Models.Status;

public class StatusDetailModel : AuditModel
{
    public string Type { get; set; }
    public string Display { get; set; }
    public string Code { get; set; }
}