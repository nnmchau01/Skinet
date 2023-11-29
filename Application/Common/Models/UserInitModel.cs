namespace Application.Common.Models;

public class UserInitModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string SecurityStamp { get; set; }
    public List<string> Roles { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string NickName { get; set; }
    public string SocialJson { get; set; }
    public string? LastLoginJson { get; set; }
    public string? Picture { get; set; }
}