using Application.Common.Models;

namespace Application.Interfaces;

public interface IIdentityService
{
    CurrentUserModel GetCurrentUserLogin();
    bool IsUnAuthorized();
    bool IsInRoleAsync(string role);
    bool IsEmailConfirmed();
}