using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ivoryservices.Models;
using Ivoryservices.viewmodel;
using System.IO;

namespace Ivoryservices.Controllers
{
    public class C_RegistrationController : Controller
    {
 private readonly ILogger<HomeController> _logger;
        private readonly OnestopContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private Stream fileStream;

        public C_RegistrationController(ILogger<HomeController> logger, OnestopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: C_Registration
        public async Task<IActionResult> Index()
        {
              return _context.C_Registrations != null ? 
              View(await _context.C_Registrations.ToListAsync()) :
              Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
        }

        // GET: C_Registration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.C_Registrations == null)
            {
                return NotFound();
            }

            var c_Registration = await _context.C_Registrations
                .FirstOrDefaultAsync(m => m.Regis_Id == id);
            if (c_Registration == null)
            {
                return NotFound();
            }

            return View(c_Registration);
        }
        //    public IActionResult C_Registrations()
        //    {
        //        return View();
        //    }
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult C_Registrations(IFormCollection collection)
        //    {
        //        try
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }


        //    // GET: C_Registration/Create
        public IActionResult Create(c_registrationsviewmodel rm)
        {
            string stringfilename = UploadFile(rm);
            var c_Registration = new C_Registration
            {
               // Cat_Name = vm.Cat_Name,
                ProfileImg = stringfilename,
                //Cat_price = vm.Cat_price
            };

            var list = new List<SelectListItem>
    {
        new SelectListItem{ Text="--Select--", Value = "0" ,Selected = true },
        new SelectListItem{ Text="User", Value = "User" },
        new SelectListItem{ Text="ServiceProvider", Value = "ServiceProvider"},
       // new SelectListItem{ Text="Admin", Value = "Admin"},
    };

            //ViewBag.ROLES = list;
            ViewData["ROLES"] = list;

            return View();
        }
        private string UploadFile(c_registrationsviewmodel rm)
        {
            string filename = null;
            if (rm.ProfileImg != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + "-" + rm.ProfileImg.FileName;
                string filepath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    rm.ProfileImg.CopyTo(fileStream);
                }
            }
            return filename;
        }

        // POST: C_Registration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Regis_Id,Regis_Name,Address,City,State,Roles,Email,Password,Confirmpassword,Status,Mobile_no,ProfileImg")] C_Registration c_Registration)
       
        //public async Task<IActionResult> Create(c_registrationsvie

        {
            if (ModelState.IsValid)
            {

                _context.Add(c_Registration);
                await _context.SaveChangesAsync();
                Login l = new Login();
                l.Regis_Id = c_Registration.Regis_Id;
                l.UserName = c_Registration.Regis_Name;
                l.Password = c_Registration.Password;
                l.Roles = c_Registration.Roles;
                _context.Add(l);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
                //return RedirectToAction(nameof(Index));
            }
           // return View(c_Registration);
           return RedirectToAction("Index", "Home");
        }

        //private string UploadedFile(c_registrationsviewmodel model)
        //{
        //    string uniqueFileName = null;

        //    if (model.ProfileImg != null)
        //    {
        //        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImg.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.ProfileImg.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}

        
        // GET: C_Registration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.C_Registrations == null)
            {
                return NotFound();
            }

            var c_Registration = await _context.C_Registrations.FindAsync(id);
            if (c_Registration == null)
            {
                return NotFound();
            }
            return View(c_Registration);
        }

        // POST: C_Registration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Regis_Id,Regis_Name,Address,City,State,Roles,Email,Password,Confirmpassword,Status,Mobile_no")] C_Registration c_Registration)
        {
            if (id != c_Registration.Regis_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(c_Registration);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction("Index");
            }
            return View(c_Registration);
        }

        // GET: C_Registration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.C_Registrations == null)
            {
                return NotFound();
            }

            var c_Registration = await _context.C_Registrations
                .FirstOrDefaultAsync(m => m.Regis_Id == id);
            if (c_Registration == null)
            {
                return NotFound();
            }

            return View(c_Registration);
        }

        // POST: C_Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.C_Registrations == null)
            {
                return Problem("Entity set 'OnestopContext.C_Registrations'  is null.");
            }
            var c_Registration = await _context.C_Registrations.FindAsync(id);
            if (c_Registration != null)
            {
                _context.C_Registrations.Remove(c_Registration);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool C_RegistrationExists(int id)
        {
          return (_context.C_Registrations?.Any(e => e.Regis_Id == id)).GetValueOrDefault();
        }

        

    }
}
