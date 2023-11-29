namespace Application.Models.Email;

public class EmailMessageModel
{
    public List<string> SendTos { get; set; }
    public string Subject { get; set; }
    public string SendByEmail { get; set; }
}