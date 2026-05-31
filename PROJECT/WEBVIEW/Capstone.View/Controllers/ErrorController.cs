using Microsoft.AspNetCore.Mvc;

namespace Capstone.View.Controllers;

public class ErrorController : Controller
{
    [Route("404")]
    public IActionResult NotFound404()
    {
        Response.StatusCode = 404;
        ViewData["Title"] = "Trang không tìm thấy";
        return View("NotFound");
    }

    [Route("error")]
    public IActionResult ServerError()
    {
        Response.StatusCode = 500;
        ViewData["Title"] = "Đã xảy ra lỗi";
        return View("~/Views/Home/Error.cshtml");
    }
}
