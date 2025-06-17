using DocumentFormat.OpenXml.Bibliography;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Ivoryservices.Helper;
using Ivoryservices.Models;
using Ivoryservices.viewmodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace Ivoryservices.Controllers
{
    public class UserController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        //private readonly IServiceProvider _serviceProvider;
        private readonly OnestopContext _context;
        Boolean flag_delete = false;
        public UserController(ILogger<HomeController> logger, OnestopContext context)
        {
            _logger = logger;
            _context = context;
            //WebHostEnvironment = webHostEnvironment;
        }

        //public IActionResult BookingCreate()
        //{
        //    //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
        //    var item = _context.SubCategories.ToList();
        //    //List<Category>? items = _context.Categories.ToList();
        //    return View(item);
        //}

        public IActionResult BookingIndex()
        {

            var item = _context.BookingMasters.ToList();
            TempData["User"] = "USER";
            return View(item.ToList());

        }
        [HttpPost]
        public IActionResult Getsubcategory(string id)
        {
            int Cat_Id;
            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CatNames = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                Cat_Id = Convert.ToInt32(id);
                List<SubCategory> subcat = _context.SubCategories.Where(x => x.Cat_Id == Convert.ToInt16(id)).ToList();
                subcat.ForEach(x =>
                {
                    CatNames.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Sub_Name, Value = x.Sub_Id.ToString() });
                });
            }
            return new JsonResult(new { data = CatNames });
            // return Json(new { response = CatNames }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //{

        //}
        [HttpGet]
        public IActionResult Getamount(string id)
        {
            int Sub_Id;
            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> SubNames = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                //Sub_Id = Convert.ToInt32(id);
                List<SubCategory> subprice = _context.SubCategories.Where(x => x.Sub_Name == id).ToList();
                subprice.ForEach(x =>
                {
                    SubNames.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Sub_Name, Value = x.Sub_Price.ToString() });
                });
            }
            return new JsonResult(new { data = SubNames });
            // return Json(new { response = CatNames }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }


        public IActionResult logout()
        {
            TempData["msg"] = "";
            TempData["Admin"] = "";
            TempData["User"] = "";
            TempData["Serviceprovider"] = "";
            TempData["User_name"] = "";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult BookingCreate(int id)
        {
            if (TempData["User_name"] == "")
            {
                return RedirectToAction("Create", "C_Registration");
            }
            else
            {
                SubCategory SubCats = new SubCategory();
                if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
                {
                    List<Item> cart = new List<Item>();
                    var cat = _context.SubCategories.SingleOrDefault(e => e.Sub_Id == id);
                    
                    cart.Add(new Item { Product = cat, Quantity = 1, tax = (cat.Sub_Price * 10) / 100, total = cat.Sub_Price + (cat.Sub_Price * 10) / 100 });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                    int index = isExist(id);
                    if (index != -1)
                    {
                        cart[index].Quantity++;
                    }
                    else
                    {

                        var cat = _context.SubCategories.SingleOrDefault(e => e.Sub_Id == Convert.ToInt32(id));
                        cart.Add(new Item { Product = cat, Quantity = 1, tax = (cat.Sub_Price * 10) / 100, total = cat.Sub_Price + (cat.Sub_Price * 10) / 100 });
                    }

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

                }
                return RedirectToAction("Index1");
            }


            //public IActionResult BookingCreate(int id)
            //{
            //    if (TempData["User_name"] == "")
            //    {
            //        return RedirectToAction("Create", "C_Registration");
            //    }
            //    else
            //    {
            //        SubCategory SubCats = new SubCategory();

            //        if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            //        {
            //            List<Item> items = new List<Item>();
            //            var cat = _context.SubCategories.SingleOrDefault(e => e.Sub_Id == id);

            //            items.Add(new Item { Product = cat, Quantity = 1 });

            //Cart cart = new Cart();
            //cart.items = items;
            //cart.grossTotal = cat.Sub_Price;
            //cart.tax = (cat.Sub_Price * 10) / 100;
            //cart.total = cat.Sub_Price + cart.tax;


            //    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cat);
            //}
            //else
            //{
            //Cart cart = SessionHelper.GetObjectFromJson<Cart>(HttpContext.Session, "cart");
            //IList<Cart> cart_items = new List<Cart>();
            //int index = isExist(id);
            //if (index != -1)
            //{
            //    var cat = _context.SubCategories.SingleOrDefault(e => e.Sub_Id == id);
            //    Item item= new Item();
            //    item.Product = cat;
            //    item.Quantity = 1;

            //    cart.items.Add(item);
            //    cart.grossTotal = cart.grossTotal + cat.Sub_Price;
            //    cart.tax = (cart.grossTotal * 10) / 100;
            //    cart.total = cart.grossTotal + cart.tax;
            //    cart_items.Add(cart);



            //}
            //else
            //{

            //    var cat = _context.SubCategories.SingleOrDefault(e => e.Sub_Id == Convert.ToInt32(id));

            //    for(var i =0; i < cart.items.Count; i++)
            //    {
            //Item item = cart.items[i];
            //if (item.Product.Sub_Id == cat.Sub_Id)
            //{
            //    item.Quantity = item.Quantity + 1;
            //    cart.items[i] = item;
            //    cart.grossTotal = cart.grossTotal + cat.Sub_Price;
            //    cart.tax = (cart.grossTotal * 10) / 100;
            //    cart.total = cart.grossTotal + cart.tax;

            //}
            //            }


            //        }
            //        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            //    }

            //    return RedirectToAction("Index1");
            //}
            //return RedirectToAction("Index1");

        }
        public IActionResult Index1()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            List<Item> SomeList = new List<Item>();
            bool deletedata=(bool)SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "flag_delete");
            if (SomeList != null && deletedata != true) 
            { 
            SomeList.Add(cart[0]);
            TempData["CCART"] = SomeList;

            ViewBag.cart = cart;
             return View();
            }
            else 
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "flag_delete", false);
                return RedirectToAction("ViewCategoryIndex");
            }
            //List<Cart> objSt = new List<Cart>();
            //objSt = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

            //ViewBag.cart = cart;
            //ViewData["Data"] = objSt;
            
        }

        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session,"cart");
            cart.RemoveAll(x => x.Product.Sub_Id == id);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
           // Session["cart"] = cart;
            //Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            if(cart.Count>0)
            return RedirectToAction("Index1");
            else
                return RedirectToAction("ViewCategoryIndex");
            // List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            // int Index1 = isExist(id);
            // cart.RemoveAt(Index1);
            // SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            // SessionHelper.SetObjectAsJson(HttpContext.Session, "flag_delete", true);
            //// flag_delete = true;
            // return RedirectToAction("Index1");
        }
        private int isExist(int id)
        {
            
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Sub_Id.Equals(id))
                {

                    return i;
                }
            }
            return -1;
        }
            //List<Item> items = new List<Item>();
            //int flag = 0;
            //var SameNames = items.All(x => items.All(y => y.Product.Sub_Id.Equals(id)));
            //if (SameNames != null)
            //    flag = 0;
            //else
            //    flag = -1;
            //List<Cart> cart = SessionHelper.GetObjectFromJson<Cart>(HttpContext.Session, "cart");

            //List<Item> items = cart.i;

            //for (int i = 0; i < It; i++)
            //{
            //    if (items[i].Product.Sub_Id.Equals(id))
            //    {
            //        return i;
            //    }
            //}
            //return flag;
        
        //ViewBag.user_name = TempData["User_name"];

        //var service_pr_data = _context.C_Registrations.Where(x => x.Roles == "ServiceProvider").Select(x => x.Regis_Name).ToList();
        ////var cat_data = _context.Categories.Where(x => x.Cat_Name == "Cat_Id").Select(x => x.Cat_Name);

        //if (service_pr_data != null)
        //{
        //    ViewBag.ser_provider = service_pr_data.ToList();
        //}
        //var Categories_items = _context.Categories.ToList();
        //if (Categories_items != null)
        //{
        //    ViewBag.Catgdata = Categories_items;
        //}
        //var SubCategories_items = _context.SubCategories.ToList();
        //if (SubCategories_items != null)
        //{
        //    ViewBag.SubCatgdata = SubCategories_items;
        //}
        //var cat = _context.BookingMasters.ToList();
        //return View(cat);



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Bookingcreate()
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
        public ActionResult Create(BookingMaster b)
        {
            return View(b);
        }

        //    var Category = _context.Categories.Where(a => a.Cat_Id == bm.Cat_Id).Select(a => a.Cat_Name).FirstOrDefault();
        //    var Subcategory = _context.SubCategories.Where(b => b.Cat_Id == bm.Cat_Id).Select(b => b.Sub_Name).FirstOrDefault();
        //    var name = @TempData["XYZ"];
        //public IActionResult BookDetail(int Cat_Id)
        //{
        //    var item = _context.BookingMasters.ToList();

        //    for(int i = 0; i < item.Count; i++)
        //    {
        //        if(item.)



        //    return View();
        //}
      
        public IActionResult BookserviceIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.BookingMasters.ToList();
            TempData["user"] = "USER";
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult GiveFeedbackIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Feedbacks.ToList();
            TempData["user"] = "USER";
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult ViewCategoryIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Categories.ToList();
            TempData["user"] = "USER";
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

        public IActionResult ViewSubCategoryIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.SubCategories.ToList();
            TempData["user"] = "USER";
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
        public IActionResult ViewSubCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewSubCategory(IFormCollection collection)
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
        public IActionResult ViewSubCategory(SubCategory vs)
        {

            var SubCategory = new SubCategory
            {
                Sub_Id = vs.Sub_Id,
                Cat_Id = vs.Cat_Id,
                //Cat_Name = vs.Cat_Name,
                Sub_Name = vs.Sub_Name,
                //Sub_price= vs.Sub_price,
                Sub_Image = vs.Sub_Image

                //  L_Id = Convert.ToInt16(TempData["loginid"])
            };
            _context.SubCategories.Add(SubCategory);
            _context.SaveChanges();
            return RedirectToAction("ViewSubCategoryIndex");
        }


        [HttpGet]
        public IActionResult GiveFeedback()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiveFeedback(IFormCollection collection)
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
        public IActionResult feedback(Feedbacks fb)
        {

            var feedbacks = new Feedbacks
            {
                Description = fb.Description,
                F_Date = fb.F_Date,
                L_Id = Convert.ToInt16(TempData["loginid"])

            };

            _context.Feedbacks.Add(feedbacks);
            _context.SaveChanges();
            return RedirectToAction("GiveFeedback");
        }

        [HttpGet]
        public async Task<IActionResult> GiveFeedbackEdit(int? id)
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
        public async Task<IActionResult> GiveFeedbackEdit(int id, [Bind("F_Id,L_Id,Description,F_Date")] Feedbacks Feedbacks)
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
                return RedirectToAction("GiveFeedbackIndex");
            }
            return View(Feedbacks);
        }


        public async Task<IActionResult> GiveFeedbackDetails(int? id)
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


        public async Task<IActionResult> GiveFeedbackDelete(int? id)
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
        [HttpPost, ActionName("GiveFeedbackDelete")]
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
            return RedirectToAction("GiveFeedbackIndex");
        }
        private bool BookserviceExists(int id)
        {
            return (_context.BookingMasters?.Any(e => e.BId == id)).GetValueOrDefault();
        }

        private bool FeedbacksExists(int id)
        {
            return (_context.Feedbacks?.Any(e => e.F_Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IActionResult GetSub(int id)
        {
            var cat = _context.SubCategories.Where(e => e.Cat_Id == id).ToList();
            return View(cat);

        }

        //public IActionResult Book(int id)
        //{
        //    var cat = _context.SubCategories.Where(e => e.Cat_Id == id).ToList();
        //    return View(cat);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSub(IFormCollection collection)
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
        public IActionResult GetSub(SubCategory sg)
        {

            var subget = new SubCategory
            {
                Sub_Id = sg.Sub_Id,
                Cat_Id = sg.Cat_Id,
                Sub_Name = sg.Sub_Name,
                Sub_Image = sg.Sub_Image,
                Sub_Price = sg.Sub_Price

                //  L_Id = Convert.ToInt16(TempData["loginid"])
            };
            // _context.Feedbacks.Add(Feedbacks);
            _context.SaveChanges();
            return RedirectToAction("BookingCreate");
        }
        public IActionResult Payment()
        {

            return View();
        }

        // public IActionResult (int id) { }



        [HttpGet]
        public IActionResult Bookservices()
        {
            return View();
        }


        [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Bookservices(int txt)
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

        public ActionResult CheckOut(List<Item> carts)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            //var Feedbacks = await _context.Feedbacks.FindAsync(id);

            BookingMaster bk = new BookingMaster();
            int count = 0;
            if (cart != null)
            {
                foreach (var result in cart)
                {
                    bk = new BookingMaster();
                   // bk.BId = count++;
                    bk.Sub_Name = result.Product.Sub_Name;
                    bk.Sub_Price = result.Product.Sub_Price;
                    bk.tax= result.tax;
                    bk.total= result.total;
                    bk.grossTotal = result.grossTotal;
                    bk.Quantity = result.Quantity;
                    bk.Order_date=DateTime.Now;
                    bk.UserName = HttpContext.Session.GetString("User_name").ToString();
                    bk.L_Id = Convert.ToInt16(TempData["loginid"]);
                    bk.booking_flag = true;
                    _context.BookingMasters.Add(bk);
                    _context.SaveChanges();
                    // bk.booking_flag = false;
                }
              //  count = 0;
                // return RedirectToAction("BookserviceIndex");
            }
            //_context.SaveChanges();
            //  await _context.SaveChangesAsync();
            // var val = Request.Form["Cart"];
            //List<Item> SomeList1 = new List<Item>();
            //SomeList1 = TempData["CCART"] as List<Item>;

            //try
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            return RedirectToAction("Invoice","Admin");

            //}
        }

        public IActionResult Bookservices(BookingMaster bm)
    {

        var BookingMaster = new BookingMaster
        {

           Sub_Name=bm.Sub_Name,
           Sub_Price=bm.Sub_Price,
           tax=bm.tax,
           total=bm.total,
           grossTotal=bm.grossTotal,    
           //Order_date=bm.Order_date,

           // L_Id = Convert.ToInt16(TempData["loginid"])

        };

        _context.BookingMasters.Add(BookingMaster);
        _context.SaveChanges();
        return RedirectToAction("BookserviceIndex");
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




