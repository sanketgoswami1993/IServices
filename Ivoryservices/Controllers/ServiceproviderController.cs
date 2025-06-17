using Ivoryservices.Models;
using Ivoryservices.viewmodel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ivoryservices.Controllers
{
    public class ServiceproviderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly OnestopContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private Stream fileStream;

        public ServiceproviderController(ILogger<HomeController> logger, OnestopContext context , IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult ViewBookingIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.BookingMasters.ToList();
            TempData["Serviceprovider"] = "SERVICEPROVIDER";
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult ViewBooking()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewBooking(IFormCollection collection)
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
        public IActionResult Create(Order_service vb)
        {

            var order_service = new Order_service
            {
                Order_Id = vb.Order_Id,
                Description = vb.Description,
                Address = vb.Address,
                City = vb.City,
                Mobile = vb.Mobile,
                BookDate = vb.BookDate,
                Status = vb.Status,
                L_Id = Convert.ToInt16(TempData["loginid"])

            };
            _context.Order_Services.Add(order_service);
            _context.SaveChanges();
            return RedirectToAction("ViewBookingIndex");
        }
        [HttpGet]
        public async Task<IActionResult> ViewBookingEdit(int? id)
        {

            if (id == null || _context.Order_Services == null)
            {
                return NotFound();
            }

            var Order_service = await _context.Order_Services.FindAsync(id);
            if (Order_service == null)
            {
                return NotFound();
            }
            return View(Order_service);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewBookingEdit(int id, [Bind("Order_Id,C_Regis_Id,Description,Address,City,Mobile,BookDate,Status")] Order_service Order_service)
        {
            if (id != Order_service.Order_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Order_service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!serviceExits(Order_service.Order_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewBookingIndex");
            }
            return View(Order_service);
        }

        private bool serviceExits(int id)
        {
            return (_context.Order_Services?.Any(e => e.Order_Id == id)).GetValueOrDefault();
        }
       
        public async Task<IActionResult> ViewBookingDetails(int? id)
        {
            
            if (id == null || _context.BookingMasters == null)
            {
                return NotFound();
            }

            var Order_service = await _context.BookingMasters
                .FirstOrDefaultAsync(m => m.BId == id);
            if (Order_service == null)
            {
                return RedirectToAction("ViewBookingIndex");
            }
           
            
            return View(Order_service);
        }

        public async Task<IActionResult> ViewBookingDelete(int? id)
        {
            if (id == null || _context.Order_Services == null)
            {
                return NotFound();
            }

            var Order_Services = await _context.Order_Services
                .FirstOrDefaultAsync(m => m.Order_Id == id);
            if (Order_Services == null)
            {
                return NotFound();
            }

            return View(Order_Services);
        }

        // POST: C_Registration/Delete/5
        [HttpPost, ActionName("ViewBookingDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (_context.Order_Services == null)
            {
                return Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
            }
            var Order_Services = await _context.Order_Services.FindAsync(id);
            if (Order_Services != null)
            {
                _context.Order_Services.Remove(Order_Services);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewBookingIndex");
        }

        public IActionResult logout()
        {
            TempData["msg"] = "";
            TempData["Admin"] = "";
            TempData["User"] = "";
            TempData["User_name"] = "";
            return RedirectToAction("Index", "Home");
        }


        public IActionResult CategoryIndex()
        {

            var item = _context.Categories.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
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


        public IActionResult Creates(categoryviewmodel vm)
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

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Cat_Id == id)).GetValueOrDefault();
        }

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


        public IActionResult SubCategoriesIndex()
        {
            var item = _context.SubCategories.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
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


        private bool SubCategoryExists(int id)
        {
            return (_context.SubCategories?.Any(e => e.Sub_Id == id)).GetValueOrDefault();
        }

        // GET: C_Registration/Edit/5
        public async Task<IActionResult> EditProfile(int? id)
        {
            //HttpContext.Session.SetString("loginid", data.L_Id.ToString());
            String UserName = HttpContext.Session.GetString("User_name").ToString();
            var Name_User = _context.C_Registrations.Where(f => f.Regis_Name.Equals(UserName));
            //var data = _context.C_Registrations.Where(a => a.Regis_Id.Equals(a.Regis_Id) && a.Regis_Name.Equals(a.Regis_Name)).FirstOrDefault();
            // var test = _context.C_Registrations.Include("Regis_Id").Include("Regis_Name").Where(c => c.Regis_Id == Regis_Id && c.Regis_Name == Regis_Name);

            if (Name_User == null || _context.C_Registrations == null)
            {
                return NotFound();
            }

            // var c_Registration = _context.C_Registrations.First(x => x.Regis_Name == Name_User);

            return View(Name_User.SingleOrDefault());
            //return RedirectToAction("C_Registration", "Edit");
        }

        // POST: C_Registration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditProfile(C_Registration c_Registration)
        {
            //if (id != c_Registration.Regis_Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(c_Registration);
                    await _context.SaveChangesAsync();
                    //var login = new Login
                    //{
                    //    Password = c_Registration.Password,
                    //    UserName = c_Registration.Regis_Name
                    //};
                    var query = _context.Logins.Where(x => x.Regis_Id == c_Registration.Regis_Id).ToList();
                    foreach (var logindata in query)
                    {

                        logindata.Password = c_Registration.Password;
                        logindata.UserName = c_Registration.Regis_Name;
                    }
                    _context.SaveChanges();




                    // _context.Update(login);
                    //await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!C_RegistrationExists(c_Registration.Regis_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("user", "Roles");
            }
            return RedirectToAction("user", "Roles");

        }

        private bool C_RegistrationExists(int id)
        {
            return (_context.C_Registrations?.Any(e => e.Regis_Id == id)).GetValueOrDefault();
        }

    }
}
