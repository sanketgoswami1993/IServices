using DocumentFormat.OpenXml.Bibliography;
using Ivoryservices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ivoryservices.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IServiceProvider _serviceProvider;
        private readonly OnestopContext _context;

        public VisitorsController(ILogger<HomeController> logger, OnestopContext context)
        {
            _logger = logger;
            _context = context;
            //WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult ViewCategoryIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Categories.ToList();
            //TempData["user"] = "USER";
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult ViewCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewCategory(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult ViewCategory(Category vc)
        {

            var Category = new Category
            {
                Cat_Id = vc.Cat_Id,
                L_Id = vc.L_Id,
                Cat_Name = vc.Cat_Name,
                Cat_Image = vc.Cat_Image,
                //Cat_price = vc.Cat_price,
                //  L_Id = Convert.ToInt16(TempData["loginid"])
            };
            _context.Categories.Add(Category);
            _context.SaveChanges();
            return RedirectToAction("ViewCategoryIndex");
        }

        //public IActionResult ViewSubCategoryIndex()
        //{
        //    //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
        //    var item = _context.SubCategories.ToList();
        //    //TempData["user"] = "USER";
        //    //List<Category>? items = _context.Categories.ToList();
        //    return View(item);
        //}
        //public IActionResult ViewSubCategory()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ViewSubCategory(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //public IActionResult ViewSubCategory(SubCategory vs)
        //{

        //    var SubCategory = new SubCategory
        //    {
        //        Sub_Id = vs.Sub_Id,
        //        Cat_Id = vs.Cat_Id,
        //        //Cat_Name = vs.Cat_Name,
        //        Sub_Name = vs.Sub_Name,
        //        //Sub_price= vs.Sub_price,
        //        Sub_Image = vs.Sub_Image

        //        //  L_Id = Convert.ToInt16(TempData["loginid"])
        //    };
        //    _context.SubCategories.Add(SubCategory);
        //    _context.SaveChanges();
        //    return RedirectToAction("ViewSubCategoryIndex");
        //}

    }
}
