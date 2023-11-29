using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class HomeController : BaseAdminController
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}