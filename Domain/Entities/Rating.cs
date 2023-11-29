namespace Domain.Entities;

public class Rating : Audit
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int StarPoint { get; set; }
}