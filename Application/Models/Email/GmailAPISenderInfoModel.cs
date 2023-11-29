namespace Application.Models.Email;

public class GmailAPISenderInfoModel
{
    public string AccessToken { get; set; }
    public string From { get; set; }
    public string SenderName { get; set; } = "";
}