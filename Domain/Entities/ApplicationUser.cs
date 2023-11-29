using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string? LastLoginJson { get; set; }
    public string? Picture { get; set; }
    public string? SocialJson { get; set; }
    public string? Introduction { get; set; }
    public string? ReasonBlocked { get; set; }
    public string? DateOfBirth { get; set; }
    public DateTime? BlockedDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string FullName { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string? NickName { get; set; }
}