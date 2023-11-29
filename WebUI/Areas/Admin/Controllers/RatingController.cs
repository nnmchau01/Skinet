using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class RatingController : BaseAdminController
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET
    public async Task<IActionResult> GetListPartial()
    {
        var data = await _ratingService.GetListRating();

        return PartialView("_RatingListPartial", data);
    }
}