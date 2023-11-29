using Application.Common.Models;

namespace Application.Models.Images;

public class ImageDetailModel : AuditModel
{
    public string? OriginLinkImage { get; set; }
    public string? LocalLinkImage { get; set; }
    public Guid ProductId { get; set; }
}