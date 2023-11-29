using System.Configuration;
using System.Net;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using WebUI;
using WebUI.WebHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebDependency(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;

    switch (response.StatusCode)
    {
        case (int)HttpStatusCode.NotFound:
            response.Redirect("/Common/NotFoundPage");
            break;
        case (int)HttpStatusCode.Unauthorized:
            response.Redirect("/#login");
            break;
        case (int)HttpStatusCode.Forbidden:
            response.Redirect("/Common/AccessDenied");
            break;
    }

    return Task.CompletedTask;
});

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Common/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseSession();

app.Use(async (context, next) =>
{
    var jwtToken = context.Session.GetString(AppConst.SessionJwtKey);
    // var jwtToken = context.Request.GetCookie(SystemConst.AppSecure);
    if (!string.IsNullOrEmpty(jwtToken))
        context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}");
    
    endpoints.MapHub<WebHub>("/realtime");
});

app.Run();