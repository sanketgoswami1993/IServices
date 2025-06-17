using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CrystalDecisions.Web;
using DocumentFormat.OpenXml.InkML;
using Ivoryservices.Models;
using Ivoryservices.viewmodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ivoryservices.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnestopContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private Stream fileStream;

        //private readonly Stream fileStream;

        public AdminController(ILogger<HomeController> logger, OnestopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }
        //public class CategoryIndex
        //{

        //}
        // GET: AdminController
        public IActionResult CategoryIndex()
        {
            
            var item = _context.Categories.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }

        public IActionResult SubCategoriesIndex()
        {
            var item = _context.SubCategories.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult FeedbackIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Feedbacks.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        //public IActionResult ProfileIndex()
        //{
        //    //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
        //    var item = _context.Profile.ToList();
        //    //List<Category>? items = _context.Categories.ToList();
        //    return View(item);
        //}
        public IActionResult ViewBookingIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Categories.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult InvoiceIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Invoices.ToList();
            //List<Category>? items = _context.Categories.ToList();

            return View(item);
        }

        // GET: AdminController/Details/5
        public async Task<IActionResult> CategoryDetails(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Cat_Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Category()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Category(IFormCollection collection)
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


        public IActionResult Create(categoryviewmodel vm)
        {
            string stringfilename = UploadFile(vm);
            var category = new Category
            {
                Cat_Name = vm.Cat_Name,

                Cat_Image = stringfilename,
                //Cat_price = vm.Cat_price,
                //     if (TempData["loginid"] != null)
                //{
                L_Id = Convert.ToInt16(TempData["loginid"])
                //     }
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Category");
        }

        private string UploadFile(categoryviewmodel vm)
        {
            string filename = null;
            if (vm.Cat_Image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + "-" + vm.Cat_Image.FileName;
                string filepath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    vm.Cat_Image.CopyTo(fileStream);
                }
            }
            return filename;
        }

        //public ActionResult UploadFile()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult Upload()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Upload(FormFile imgfile)
        //{

        //    try
        //    {
        //        string ext = Path.GetExtension(imgfile.FileName);
        //        System.IO.Stream stream;
        //        if (ext == ".jpg")
        //        {

        //            string fileExtension = System.IO.Path.GetExtension(imgfile.FileName);
        //            // var filename = ContentDispositionHeaderValue.Parse(BatchId.ContentDisposition).FileName.Trim('"');
        //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imgfile.FileName);
        //            using (stream = new FileStream(path, FileMode.Create))
        //            {
        //                await imgfile.CopyToAsync(stream);
        //            }
        //        }
        //        ViewBag.Message = "File Uploaded Successfully!!";
        //        return View();
        //    }
        //    catch
        //    {
        //        ViewBag.Message = "File upload failed!!";
        //        return View();
        //    }
        //    }

        //public ActionResult Detais(int id)
        //{
        //    return View();
        //}

        // GET: AdminController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AdminController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
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

        // GET: AdminController/Edit/5
        [HttpGet]
        public async Task<IActionResult> CategoryEdit(int? id)
        {

            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryEdit(int id, [Bind("Cat_Id,L_Id,Cat_Name,Cat_Image,Cat_price")] Category category)
        {
            if (id != category.Cat_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Cat_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CategoryIndex");
            }
            return View(category);
        }







        [HttpGet]
        public async Task<IActionResult> FeedbackEdit(int? id)
        {

            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var Feedbacks = await _context.Feedbacks.FindAsync(id);
            if (Feedbacks == null)
            {
                return NotFound();
            }
            return View(Feedbacks);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedbackEdit(int id, [Bind("F_Id,Description,F_Date")] Feedbacks Feedbacks)
        {
            if (id != Feedbacks.F_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Feedbacks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbacksExists(Feedbacks.F_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("FeedbackIndex");
            }
            return View(Feedbacks);
        }


        public async Task<IActionResult> FeedbackDetails(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var Feedbacks = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.F_Id == id);
            if (Feedbacks == null)
            {
                return NotFound();
            }

            return View(Feedbacks);
        }


       
        public IActionResult Feedbacks()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedbacks(IFormCollection collection)
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
        public IActionResult Creates(Feedbacks fb)
        {

            var feedbacks = new Feedbacks
            {
                Description = fb.Description,
                F_Date = fb.F_Date,
                L_Id = Convert.ToInt16(TempData["loginid"])

            };

            _context.Feedbacks.Add(feedbacks);
            _context.SaveChanges();
            return RedirectToAction("FeedbackIndex");
        }



        //public IActionResult Profile()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Profile(IFormCollection collection)
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


        //public IActionResult ReCreate(profileviewmodel pf)
        //{
        //    string stringfilename = UploadFile(pf);
        //    var profile = new Profile
        //    {
        //        //Id = pf.Id,
        //        Name = pf.Name,
        //        Image = stringfilename
        //    };
        //    _context.Profile.Add(profile);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //private string UploadFile(profileviewmodel pf)
        //{
        //    string filename = null;
        //    if (pf.Image != null)
        //    {
        //        //string fileExtension = System.IO.Path.GetExtension(imgfile.FileName);
        //        string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "images");
        //        filename = Convert.ToString(Guid.NewGuid()).ToString() + "-" + pf.Image.FileName;
        //        string filepath = Path.Combine(uploadDir, filename);
        //       using (System.IO.Stream fileStream = new FileStream(filepath, FileMode.Create))
        //        //using (FileStream fs = System.IO.File.Create(filepath))
        //        // (System.IO.Stream fileStream = new FileStream(path, FileMode.Create))
        //         {
        //            pf.Image.CopyTo(fileStream);
        //         }

        //    }
        //    return filename;
        //}

        //public IActionResult ReCreate(profileviewmodel pv)
        //{
        //    string stringfilename = UploadFile(pv);
        //    var profile = new Profile
        //    {
        //        //Cat_Name = vm.Cat_Name,
        //        Name = pv.Name,
        //        Image = stringfilename,
        //    };
        //    _context.Profile.Add(profile);
        //    _context.SaveChanges();
        //    return RedirectToAction("ProfileIndex");
        //}

        //private string UploadFile(profileviewmodel pv)
        //{
        //    string filename = null;
        //    if (pv.Image != null)
        //    {
        //        string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "images");
        //        filename = Guid.NewGuid().ToString() + "-" + pv.Image.FileName;
        //        string filepath = Path.Combine(uploadDir, filename);
        //        using (var fileStream = new FileStream(filepath, FileMode.Create))
        //        {
        //            pv.Image.CopyTo(fileStream);
        //        }
        //    }
        //    return filename;
        //}

        //[HttpGet]
        //public async Task<IActionResult> ProfileEdit(int? id)
        //{

        //    if (id == null || _context.Profile == null)
        //    {
        //        return NotFound();
        //    }

        //    var Profile = await _context.Profile.FindAsync(id);
        //    if (Profile == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Profile);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ProfileEdit(int id, [Bind("Id,Name,Image")] Profile Profile)
        //{
        //    if (id != Profile.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(Profile);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProfileExists(Profile.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("ProfileIndex");
        //    }
        //    return View(Profile);
        //}

        private bool InvoiceExists(int id)
        {
            return (_context.Invoices?.Any(e => e.In_Id == id)).GetValueOrDefault();
        }
        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Cat_Id == id)).GetValueOrDefault();
        }
        private bool SubCategoryExists(int id)
        {
            return (_context.SubCategories?.Any(e => e.Sub_Id == id)).GetValueOrDefault();
        }
        private bool FeedbacksExists(int id)
        {
            return (_context.Feedbacks?.Any(e => e.F_Id == id)).GetValueOrDefault();
        }
        private bool ReportExists(int id)
        {
            return (_context.Feedbacks?.Any(e => e.F_Id == id)).GetValueOrDefault();
        }

        //private bool ProfileExists(int id)
        //{
        //    return (_context.Profile?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Cat_Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: C_Registration/Delete/5
        [HttpPost, ActionName("CategoryDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CategoryIndex");
        }

        public async Task<IActionResult> FeedbackDelete(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var Feedbacks = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.F_Id == id);
            if (Feedbacks == null)
            {
                return NotFound();
            }

            return View(Feedbacks);
        }

        // POST: C_Registration/Delete/5
        [HttpPost, ActionName("FeedbackDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
            }
            var Feedbacks = await _context.Feedbacks.FindAsync(id);
            if (Feedbacks != null)
            {
                _context.Feedbacks.Remove(Feedbacks);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("FeedbackIndex");
        }



        //public async Task<IActionResult> ProfileDelete(int? id)
        //{
        //    if (id == null || _context.Profile == null)
        //    {
        //        return NotFound();
        //    }

        //    var Profile = await _context.Profile
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (Profile == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(Profile);
        //}

        //// POST: C_Registration/Delete/5
        //[HttpPost, ActionName("ProfileDelete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConf(int id)
        //{
        //    if (_context.Profile == null)
        //    {
        //        return Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
        //    }
        //    var Profile = await _context.Profile.FindAsync(id);
        //    if (Profile != null)
        //    {
        //        _context.Profile.Remove(Profile);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ProfileIndex");
        //}

        public IActionResult SubCategories()
        {
            var Categories_items = _context.Categories.ToList();
            if (Categories_items != null)
            {
                ViewBag.Catgdata = Categories_items;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubCategories(subcategoriesviewmodel sc)
        {
            string stringfilename = UploadFile(sc);
            var subcategory = new SubCategory
            {

                Cat_Id = sc.Cat_Id,
                Sub_Name = sc.Sub_Name,
                Sub_Price = sc.Sub_Price,
                Sub_Image = stringfilename,

            };

            _context.SubCategories.Add(subcategory);
            _context.SaveChanges();
            return RedirectToAction("SubCategories");
        }

        private string UploadFile(subcategoriesviewmodel sc)
        {
            string filename = null;
            if (sc.Sub_Image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + "-" + sc.Sub_Image.FileName;
                string filepath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    sc.Sub_Image.CopyTo(fileStream);
                }
            }
            return filename;
        }


        public async Task<IActionResult> SubCategoriesDelete(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(m => m.Sub_Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return View(subcategory);
        }

        [HttpPost, ActionName("SubCategoriesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubConfirm(int id)
        {
            if (_context.SubCategories == null)
            {
                return Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
            }
            var subcategory = await _context.SubCategories.FindAsync(id);
            if (subcategory != null)
            {
                _context.SubCategories.Remove(subcategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("SubCategoriesIndex");
        }

        [HttpGet]
        public async Task<IActionResult> SubCategoriesEdit(int? id)
        {

            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subcategory = await _context.SubCategories.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubCategoriesEdit(int id, [Bind("Cat_Id,Sub_Id,Sub_Name,Sub_Image,Sub_Price")] SubCategory Subcategory)
        {
            if (id != Subcategory.Sub_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Subcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(Subcategory.Sub_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SubCategoriesIndex");
            }
            return View(Subcategory);
        }

        public async Task<IActionResult> SubCategoriesDetails(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(m => m.Sub_Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return View(subcategory);
        }

        public IActionResult subcategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult subcategory(IFormCollection collection)
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

        [HttpGet]
        public IActionResult Invoice()
        {

            var Invoices_items = _context.BookingMasters.ToList();
            //var query = _context.BookingMasters.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString()).ToList();
            //foreach (var order in query)
            //{
            //    var b =  _context.BookingMasters
            //    .FirstOrDefaultAsync(b =>b.booking_flag == false);
            //}
            //_context.SaveChanges();
            //var Invoices_items = _context.BookingMasters.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == false);





            // var Invoices_items = _context.BookingMasters.Where(a => a.booking_flag.Equals("True")).FirstOrDefault();

            // var data = _context.Admins.
            //if (Invoices_items != null)
            //{
            //    ViewBag.Catgdata = Invoices_items;
            //bk.UserName = Convert.ToString(TempData["user"]);
            //bk.L_Id = Convert.ToInt16(TempData["loginid"]);
            //}
            HttpContext.Session.SetString("Tot_amt", Invoices_items.Where(x => x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == true).Select(x => x.total).Sum().ToString());
            ViewBag.TotalAmount = Invoices_items.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == true).Select(x => x.total).Sum();
            //            _context.BookingMasters
            //.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString())
            //.ToList()
            //.ForEach(x => x.booking_flag = false);
            //            _context.SaveChanges();
            return View(Invoices_items.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == true));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Invoice(Invoice ci)
        //{

        //    var invoice = new Invoice
        //    {

        //      Sub_Id = ci.Sub_Id,
        //      Sub_Name = ci.Sub_Name,
        //      Sub_Price = ci.Sub_Price,
        //      tax=ci.tax,
        //      total=ci.total, 
        //      Order_date=ci.Order_date,        
        //    };

        //    _context.Invoices.Add(invoice);
        //    _context.SaveChanges();
        //    return RedirectToAction("InvoiceIndex");
        //}
        ////public JsonResult UIndex()
        //{
        //    var data = _context.C_Registrations.ToList();
        //    return new JsonResult(data);
        //}

        public async Task<IActionResult> UIndex()
        {
            return _context.C_Registrations != null ?
                        View(await _context.C_Registrations.ToListAsync()) :
                        Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
            //return RedirectToAction("UIndex", "Admin");
        }

        public IActionResult logout()
        {
            TempData["msg"] = "";
            TempData["Admin"] = "";
            TempData["User"] = "";
            TempData["User_name"] = "";
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Report()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(IFormCollection collection)
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
        //public IActionResult Report(Report rp)
        //{

        //    return RedirectToAction("ReportIndex");
        //}

    }
}

