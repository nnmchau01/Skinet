using Application.Common.Models;
using Application.Models.Role;

namespace Application.Models.User;

public class UserDetailModel : AuditModel
{
    public UserDetailModel()
    {
        RolesDefault = new List<RoleDetailModel>();
    }

    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Roles { get; set; }
    public IList<string>? ListRole { get; set; }
    public List<RoleDetailModel> RolesDefault { get; set; }
}