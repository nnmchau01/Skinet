using Application.Interfaces;
using Application.Models.Rating;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class RatingController : BaseController
{
    private readonly IRatingService _ratingService;
    private readonly IIdentityService _identityService;

    public RatingController(IRatingService ratingService, IIdentityService identityService)
    {
        _ratingService = ratingService;
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> Detail([FromBody] RatingFilterModel model)
    {
        if (string.IsNullOrEmpty(_identityService.GetCurrentUserLogin().Id))
        {
            return Json(0);
        }
        
        model.UserId = Guid.Parse(_identityService.GetCurrentUserLogin().Id);
        var data = await _ratingService.GetRatingDetail(model);

        return Json(data);
    }
    
    [Authorize(Roles = SecurityRoles.User)]
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] RatingUpdateModel model)
    {
        model.UserId = Guid.Parse(_identityService.GetCurrentUserLogin().Id);
        var data = await _ratingService.UpdateRating(model);

        return Json(data);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetAverageStarPoint([FromBody] RatingFilterModel model)
    {
        var averageStarPoint = await _ratingService.GetAverageStarPoint(model);

        return Json(averageStarPoint);
    }
}