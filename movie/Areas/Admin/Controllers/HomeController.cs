using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Web.Areas.Admin.Data;
using Movie_Web.Areas.Admin.Models;
using Movie_Web.Models;

namespace Movie_Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {

        private readonly MoviesContext _context;
        public HomeController(MoviesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var danhgia = new DanhGia();
            danhgia.TongTheLoai = _context.Categories.Count();
            danhgia.TongLoaiPhim = _context.Types.Count();
            danhgia.TongQuocGia = _context.Countries.Count();
            danhgia.TongSoPhim = _context.Movies.Count();

            var SoLuotXemThang = new LuotXemThang();
            SoLuotXemThang.SoLuotXem_1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_1").Value;
            SoLuotXemThang.SoLuotXem_2 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_2").Value;
            SoLuotXemThang.SoLuotXem_3 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_3").Value;
            SoLuotXemThang.SoLuotXem_4 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_4").Value;
            SoLuotXemThang.SoLuotXem_5 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_5").Value;
            SoLuotXemThang.SoLuotXem_6 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_6").Value;
            SoLuotXemThang.SoLuotXem_7 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_7").Value;
            SoLuotXemThang.SoLuotXem_8 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_8").Value;
            SoLuotXemThang.SoLuotXem_9 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_9").Value;
            SoLuotXemThang.SoLuotXem_10 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_10").Value;
            SoLuotXemThang.SoLuotXem_11 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_11").Value;
            SoLuotXemThang.SoLuotXem_12 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_12").Value;

            ViewBag.SoLuotXem = SoLuotXemThang;

            ViewBag.DanhGia = danhgia;
            return View();
        }
    }
}
