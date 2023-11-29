using System.ComponentModel.DataAnnotations;

namespace Application.Models.User;

public class UpdateUserRole
{
    [Required] public string Id { get; set; }
    [Required] public List<string> Roles { get; set; }
}