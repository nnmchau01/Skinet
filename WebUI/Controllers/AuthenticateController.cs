using Application.Models.Authentication;
using Application.Models.User;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Helpers;
using Mapster;
using WebUI.WebHelper;

namespace WebUI.Controllers;

public class AuthenticateController : BaseController
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(SignInManager<ApplicationUser> signInManager
        , IConfiguration configuration
        , UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return Redirect("/");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] AddNewUserModel request)
    {
        var validation = ValidationHelper<AddNewUserModel>.IsValid(request);
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
                : $"Email {request.UserName} đã được liên kết với tài khoản khác";
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

        await _userManager.CreateAsync(identityUser, request.Password);

        List<string> defaultRoles = new() { SecurityRoles.User };

        await _userManager.AddToRolesAsync(identityUser, defaultRoles);

        var token = TokenHelper.GenerateToken(
            _configuration["JWT:Secret"]
            , _configuration["JWT:ValidIssuer"]
            , _configuration["JWT:ValidAudience"]
            , defaultRoles
            , identityUser);

        HttpContext.Session.SetString(AppConst.SessionJwtKey, token);

        var msg = AppVersion.IsEnglishVersion
            ? "Sign up success"
            : "Đăng ký thành công";

        return SuccessResponse(msg);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] LoginModel request)
    {
        var validation = ValidationHelper<LoginModel>.IsValid(request);
        if (!validation.IsValid) return BadRequestResponse(validation.Message);

        ApplicationUser user;

        if (StringHelper.ContainsAnyCase(request.UserName, '@'))
        {
            user = await _userManager.FindByEmailAsync(request.UserName);
        }
        else
        {
            user = await _userManager.FindByNameAsync(request.UserName);
        }

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var message = AppVersion.IsEnglishVersion
                ? $"Invalid Username or Password"
                : $"Tài khoản hoặc mật khẩu không đúng";
            return BadRequestResponse(message);
        }

        if (user.IsDeleted)
        {
            var message = AppVersion.IsEnglishVersion
                ? $"Your account has been deleted"
                : $"Tài khoản đã bị khóa";
            return BadRequestResponse(message);
        }

        if (!user.IsActive)
        {
            return BadRequestResponse("Your account has been deactivate");
        }

        IList<string>? userRoles = await _userManager.GetRolesAsync(user);
        //var selectClaims = await _userManager.GetClaimsAsync(user);

        var token = TokenHelper.GenerateToken(
            _configuration["JWT:Secret"]
            , _configuration["JWT:ValidIssuer"]
            , _configuration["JWT:ValidAudience"]
            , userRoles
            , user);

        HttpContext.Session.SetString(AppConst.SessionJwtKey, token);

        var msg = AppVersion.IsEnglishVersion
            ? "Login success"
            : "Đăng nhập thành công";

        return SuccessResponse(msg);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Authenticate", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
    {
        try
        {
            var userExternal = await _signInManager.GetExternalLoginInfoAsync();
            var toNewUser = ExternalLoginHelper.ToNewUser(userExternal);
            var provider = userExternal.ProviderDisplayName;
            var userExists = await _userManager.FindByEmailAsync(toNewUser.Email);
            ApplicationUser user;

            if (userExists != null)
            {
                user = userExists;
            }
            else
            {
                await _userManager.CreateAsync(toNewUser, StringHelper.PasswordGenerate());

                var newUser = await _userManager.FindByEmailAsync(toNewUser.Email);
                List<string> defaultRoles = new() { SecurityRoles.User };

                await _userManager.AddToRolesAsync(newUser, defaultRoles);

                user = newUser;
            }

            if (user.IsDeleted)
            {
                return Redirect("/Common/AccountDeleted");
            }

            IList<string>? userRoles = await _userManager.GetRolesAsync(user);
            //var selectClaims = await _userManager.GetClaimsAsync(user);

            var token = TokenHelper.GenerateToken(
                _configuration["JWT:Secret"]
                , _configuration["JWT:ValidIssuer"]
                , _configuration["JWT:ValidAudience"]
                , userRoles
                , user);

            HttpContext.Session.SetString(AppConst.SessionJwtKey, token);

            return Redirect("~/");
        }
        catch (Exception)
        {
            return Redirect($"~/#error-oauth2");
        }
    }
}