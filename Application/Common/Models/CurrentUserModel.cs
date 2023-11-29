namespace Application.Common.Models;

public class CurrentUserModel
{
    public string? UserName { get; set; }
    public string Id { get; set; }
    public string? FullName { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? NickName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Picture { get; set; }
}