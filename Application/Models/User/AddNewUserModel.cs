using System.ComponentModel.DataAnnotations;

namespace Application.Models.User;

public class AddNewUserModel
{
    [Required] public string UserName { get; set; }
    [Required] public string FullName { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Address { get; set; }
    [Required] public string PhoneNumber { get; set; }
    [Required] public string Password { get; set; }
}