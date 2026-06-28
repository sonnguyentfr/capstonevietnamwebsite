using Microsoft.AspNetCore.Mvc;

namespace Capstone.View.Controllers;

public class GioiThieuController : Controller
{
    // /gioi-thieu
    public IActionResult Index()
    {
        return View();
    }

    // /gioi-thieu/ve-capstone
    public IActionResult VeCapstone()
    {
        return View();
    }
    // /gioi-thieu/quy-trinh-tu-van
    public IActionResult QuyTrinhTuVan() => View();
}
