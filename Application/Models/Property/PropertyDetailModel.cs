using Application.Common.Models;

namespace Application.Models.Property;

public class PropertyDetailModel : AuditModel
{
    public string Name { get; set; }
    public string Value { get; set; }
    public Guid? ProductId { get; set; }
}