namespace Application.Common.Options;

public class NotifyGoogleGmailApiOption
{
    public string GmailAPIUrl { get; set; }
    public string Oauth2Url { get; set; }
    public string RefreshToken { get; set; }
    public string ClientID { get; set; }
    public string ClientSecret { get; set; }
    public string GrandType { get; set; }
    public string SenderName { get; set; }
    public string From { get; set; }
}