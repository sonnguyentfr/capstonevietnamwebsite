using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.ViewComponents;

public class TuVanDangKyViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View(new TuVanFormInputViewModel());
    }
}