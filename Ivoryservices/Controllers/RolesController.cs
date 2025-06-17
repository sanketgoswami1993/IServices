using Google.Apis.Admin.Directory.directory_v1.Data;
using ICSharpCode.Decompiler.CSharp.Syntax;
using Ivoryservices.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ivoryservices.Controllers
{
    public class RolesController : Controller
    {
        //[Authorize(Roles = "User")]
        public IActionResult User()
        {
           
            return View();
        }
        
        public IActionResult Admin()
        {
            //TempData["Admin"] = "ADMIN";
            //TempData["User_name"]
            return RedirectToAction("UIndex", "Admin");


        }
        public IActionResult Serviceprovider()
        {

            return View();
        }
        
    }
}
