using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Images;
using Application.Models.Product;
using Application.Models.Property;
using Application.Models.Role;
using Application.Models.User;
using Domain.Constants;
using Domain.Entities;
using Domain.Helpers;
using Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.WebHelper;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Admin)]
public class UserController : BaseAdminController
{
    private readonly IUserService _userService;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(
          IUserService userService
        , RoleManager<ApplicationRole> roleManager
        , UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _userService.GetUsers();

        return PartialView("_UserListPartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _userService.Delete(request.Id.ToString());

        return success ? SuccessResponse("Saved successfully") : BadRequestResponse("Saved fail");
    }

    // GET
    public async Task<IActionResult> AddNewUserPartial()
    {
        var data = await _roleManager.Roles.ToListAsync();
        var roles = data.Adapt<List<RoleDetailModel>>();

        return PartialView("_AddNewPartial", roles);
    }

    // GET
    public async Task<IActionResult> DetailPartial(string id)
    {
        var data = await _userService.GetDetail(id);

        return PartialView("_UserDetailPartial", data);
    }

    // GET
    public async Task<IActionResult> UpdateRolePartial(string id)
    {
        var data = await _userService.GetDetail(id);
        var roles = await _roleManager.Roles.ToListAsync();
        data.RolesDefault = roles.Adapt<List<RoleDetailModel>>();

        return PartialView("_UpdateRolePartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAddUser([FromBody] AdminAddNewUserModel request)
    {
        var validation = ValidationHelper<AdminAddNewUserModel>.IsValid(request);
        if (!validation.IsValid) return BadRequestResponse(validation.Message);

        var existUsername = await _userManager.FindByNameAsync(request.UserName);

        if (existUsername != null)
        {
            var message = AppVersion.IsEnglishVersion
                ? $"Username {request.UserName} has been already"
                : $"Tài khoản {request.UserName} đã tồn tại";
            return BadRequestResponse(message);
        }

        var existEmail = await _userManager.FindByEmailAsync(request.Email);

        if (existEmail != null)
        {
            var message = AppVersion.IsEnglishVersion
                ? $"Email {request.Email} has been associate with another account"
                : $"Email {request.Email} đã được liên kết với tài khoản khác";
            return BadRequestResponse(message);
        }

        var identityUser = request.Adapt<ApplicationUser>();
        identityUser.FullName = request.FullName;
        identityUser.IsActive = true;
        identityUser.IsDeleted = false;
        identityUser.Picture = AppConst.DefaultAvatar;
        identityUser.CreatedAt = DateTime.Now;
        identityUser.CreatedBy = request.UserName;
        identityUser.SocialJson = "[]";
        identityUser.LastLoginJson = "{}";
        identityUser.NickName = Guid.NewGuid().ToString();
        identityUser.SecurityStamp = Guid.NewGuid().ToString();
        identityUser.EmailConfirmed = false;
        identityUser.PhoneNumberConfirmed = false;
        identityUser.TwoFactorEnabled = false;
        identityUser.LockoutEnabled = false;
        identityUser.AccessFailedCount = 0;

        var result = await _userManager.CreateAsync(identityUser, request.Password);

        if (!result.Succeeded)
        {
            var msgFail = AppVersion.IsEnglishVersion
            ? $"Saved fail: {result.Errors.First().Description}"
            : $"Lưu thất bại: {{result.Errors.First().Description}} ";

            return BadRequestResponse(msgFail);
        }

        await _userManager.AddToRolesAsync(identityUser, request.Roles);

        var msg = AppVersion.IsEnglishVersion
            ? "Saved successfully"
            : "Lưu thành công";

        return SuccessResponse(msg);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdateRole([FromBody] UpdateUserRole request)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            var message = AppVersion.IsEnglishVersion
                ? "Not found user"
                : "Tài khoản không tồn tại";
            return BadRequestResponse(message);
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(user, userRoles);
        await Task.Delay(500);
        await _userManager.AddToRolesAsync(user, request.Roles);

        user.UpdatedAt = DateTime.Now;

        await _userManager.UpdateAsync(user);

        await Task.Delay(500);

        var msg = AppVersion.IsEnglishVersion
            ? "Saved successfully"
            : "Lưu thành công";

        return SuccessResponse(msg);
    }
}