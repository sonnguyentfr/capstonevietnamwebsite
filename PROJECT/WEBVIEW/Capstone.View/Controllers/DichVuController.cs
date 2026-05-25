using Microsoft.AspNetCore.Mvc;

namespace Capstone.View.Controllers;

public class DichVuController : Controller
{
    // /capstone-vietnam/cac-dich-vu-capstone
    public IActionResult Index() => View();

    // /tu-van-dinh-cu
    public IActionResult TuVanDinhCu() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/tu-van-du-hoc-cac-nuoc
    public IActionResult TuVanDuHocCacNuoc() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/tu-van-du-hoc-truong-top
    public IActionResult TuVanDuHocTruongTop() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/tu-van-du-hoc-cao-hoc
    public IActionResult TuVanDuHocCaoHoc() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/tu-van-nganh-nghe
    public IActionResult TuVanNganhNghe() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/tu-van-visa-du-hoc-tham-than
    public IActionResult TuVanVisa() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/dich-vu-chuyen-tien-du-hoc
    public IActionResult ChuyenTienDuHoc() => View();

    // /capstone-vietnam/cac-dich-vu-capstone/dich-vu-tim-nha
    public IActionResult TimNha() => View();
}
