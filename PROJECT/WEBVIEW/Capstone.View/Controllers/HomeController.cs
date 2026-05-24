using Microsoft.AspNetCore.Mvc;

namespace Capstone.View.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _http;

    public HomeController(IHttpClientFactory http)
    {
        _http = http;
    }

    public IActionResult Index()
    {
        // TODO: goi API lay du lieu trang chu
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
