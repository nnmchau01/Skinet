namespace Application.Models.Rating;

public class RatingUpdateModel
{
    public Guid ProductId { get; set; }
    public int StarPoint { get; set; }
    public Guid UserId { get; set; }
}