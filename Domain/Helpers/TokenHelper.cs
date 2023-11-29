using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Constants;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Helpers;

public static class TokenHelper
{
    public static string GenerateToken(string jwtSecret, string issuer, string audience
        , IList<string> userRoles, ApplicationUser user)
    {
        List<Claim> authClaims = new();
        List<Claim> claimRoles = userRoles.Select(s => new Claim(AppJwtClaimTypes.Roles, s)).ToList();

        authClaims.AddRange(new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString().ToLower()),
            new(AppJwtClaimTypes.Subject, user.Id.ToString().ToLower()),
            new(AppJwtClaimTypes.UserName, user.UserName),
            new(AppJwtClaimTypes.FullName, user.FullName),
            new(AppJwtClaimTypes.NickName, user.NickName),
            new(AppJwtClaimTypes.Email, user.Email),
            new(AppJwtClaimTypes.Picture,
                string.IsNullOrEmpty(user.Picture) ? AppConst.DefaultAvatar : user.Picture),
            new(AppJwtClaimTypes.ConfirmedEmail, user.EmailConfirmed.ToString().ToLower()),
            new(AppJwtClaimTypes.DateOfBirth, user.DateOfBirth ?? "")
        });

        authClaims.AddRange(claimRoles);

        SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(jwtSecret));

        JwtSecurityToken token = new(
            issuer,
            audience,
            expires: DateTime.Now.AddDays(365),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static DateTime GetValidTo(string jwt)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwtSecurityToken = handler.ReadJwtToken(jwt);
        return jwtSecurityToken.ValidTo;
    }
}