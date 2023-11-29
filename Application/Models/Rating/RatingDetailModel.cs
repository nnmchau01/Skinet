using Application.Common.Models;

namespace Application.Models.Rating;

public class RatingDetailModel : AuditModel
{
    public Guid ProductId { get; set; }
    public string Url { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
    public string DefaultImage { get; set; }
    public string ProductName { get; set; }
    public double AverageRatingStart { get; set; }
}