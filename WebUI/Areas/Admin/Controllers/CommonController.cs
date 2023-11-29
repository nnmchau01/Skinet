using Application.Common.Models;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = SecurityRoles.Manager)]
public class CommonController : BaseAdminController
{
    public IActionResult GetDataDropDown(string dataType)
    {
        var data = new List<DropdownModel>();
        if (dataType.Equals("Year"))
        {
            var currentYear = DateTime.Now.Year;
            for (var year = 2023; year <= currentYear; year++)
            {
                data.Add(new DropdownModel { Id = year.ToString(), Text = year.ToString() });
            }
        }
        else if (dataType.Equals("Month"))
        {
            data = new List<DropdownModel>
            {
                new DropdownModel { Id = "1", Text = "January" },
                new DropdownModel { Id = "2", Text = "February" },
                new DropdownModel { Id = "3", Text = "March" },
                new DropdownModel { Id = "4", Text = "April" },
                new DropdownModel { Id = "5", Text = "May" },
                new DropdownModel { Id = "6", Text = "June" },
                new DropdownModel { Id = "7", Text = "July" },
                new DropdownModel { Id = "8", Text = "August" },
                new DropdownModel { Id = "9", Text = "September" },
                new DropdownModel { Id = "10", Text = "October" },
                new DropdownModel { Id = "11", Text = "November" },
                new DropdownModel { Id = "12", Text = "December" }
            };
        }

        return Json(data);
    }
}