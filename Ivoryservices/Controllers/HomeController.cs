//@model CoreLogin.Models.C_Registration


using Google.Apis.Admin.Directory.directory_v1.Data;
using Ivoryservices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ivoryservices.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnestopContext _context;
        string actionename = "";
        string controller = "Roles";
      

        public HomeController(ILogger<HomeController> logger, OnestopContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public IActionResult Index()
        {
            if (TempData["msg"] !=null)
                //ViewBag.msg="Y";
                ViewBag.DisplayLinkStatus = "hidden";
            return View();
        }
        
        [HttpPost]
      //  [Authorize(Roles = "Admin,User,Serviceprovider")]
        public IActionResult Index([Bind] Login C, bool checkResp)
        {
            if(checkResp)
            {
                
                var data = _context.Admins.Where(a => a.Name.Equals(C.UserName) && a.Password.Equals(C.Password)).FirstOrDefault();
                //var data = _context.Admins.ToList();

                actionename = "Admin";

                TempData["msg"] = "You are welcome to Admin Section";
                TempData["Admin"] = "ADMIN";
                //TempData["User"] = "USER";
                TempData["loginid"] = data.L_Id;
                TempData["User_name"] = C.UserName;
                return RedirectToAction(actionename, controller);
            }
            else
            {
                var data = _context.Logins.Where(a => a.UserName.Equals(C.UserName) && a.Password.Equals(C.Password)).FirstOrDefault();
                
                if (data != null && data.Roles == "User")
                {
                    actionename = "user";

                    TempData["msg"] = "You are welcome to Admin Section";
                    TempData["loginid"] = data.L_Id;
                    HttpContext.Session.SetString("loginid", data.L_Id.ToString());
                    TempData["User"] = "USER";
                    TempData["User_name"] = C.UserName;
                    HttpContext.Session.SetString("User_name", C.UserName);
                    return RedirectToAction(actionename, controller);
                }
                else if (data != null && data.Roles == "ServiceProvider")
                {

                    actionename = "Serviceprovider";

                    TempData["msg"] = "ServiceProvider id or Password is wrong.!";
                    TempData["Serviceprovider"] = "SERVICEPROVIDER";
                    TempData["loginid"] = data.L_Id;
                    HttpContext.Session.SetString("User_name", C.UserName);
                    TempData["User_name"] = C.UserName;
                    return RedirectToAction(actionename, controller);
                }
                else if (data != null && data.Roles == "Admin")
                {

                    actionename = "Admin";                   
                    TempData["msg"] = "Admin id or Password is wrong.!";
                    return RedirectToAction(actionename, controller);
                }
                else
                {
                    actionename = "Index";
                    TempData["msg"] = "User name or password is wrong, Please check.!";
                    return RedirectToAction(actionename, "Home");
                }
            }
            //return View();
            //return RedirectToAction(actionename, controller);
        }

       

            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LoginIndex()
        {
            //var list = connection.Query<Category>("dbo.Category_GetAll").ToList();
            var item = _context.Logins.ToList();
            //List<Category>? items = _context.Categories.ToList();
            return View(item);
        }
    }

}