using Application.Common.Models;

namespace Application.Models.Status;

public class AddNewStatusModel : AuditModel
{
    public string Type { get; set; }
    public string Display { get; set; }
    public string Code { get; set; }
}