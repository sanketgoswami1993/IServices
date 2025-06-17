using Braintree;
using Ivoryservices.Models;
using Ivoryservices.Services;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ivoryservices.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBraintreeService _braintreeService;
        private readonly OnestopContext _context;
        public PaymentController(IBraintreeService braintreeService, OnestopContext context)
        {
            _braintreeService = braintreeService;
            _context = context;
        }

        public IActionResult PIndex()
        {
            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();  //Genarate a token
            ViewBag.ClientToken = clientToken;


            double x = Convert.ToDouble(HttpContext.Session.GetString("Tot_amt").ToString());

            var data = new BookPurchaseVM
            {
                P_Id = 2,
                DateTime = DateTime.Now,
                Totalprice = x,
                Nonce = ""
            };

            return View(data);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payments(BookPurchaseVM b)
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
        public IActionResult upayment(Payment py)
        {

            var payment = new Payment
            {
                // P_Id = py.P_Id,
                //HttpContext.Session.SetString("Tot_amt", Invoices_items.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == true).Select(x => x.total).Sum().ToString());
                Totalprice = Convert.ToDouble(HttpContext.Session.GetString("Tot_amt")),
                DateTime = DateTime.Now

            };

            _context.Payment.Add(payment);
            _context.SaveChanges();
            return RedirectToAction("PIndex");
        }

        private bool PaymentExists(int id)
        {
            return (_context.Payment?.Any(e => e.P_Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult Create(BookPurchaseVM model)
        {
            var gateway = _braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(Convert.ToDouble(HttpContext.Session.GetString("Tot_amt"))),
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            string uname = HttpContext.Session.GetString("User_name").ToString();
            ////string loginid= HttpContext.Session.GetString("login_id").ToString();
            //var query = _context.BookingMasters.Where(x => x.UserName == uname).ToList();
            //foreach (var b in query)
            //{
            //    b.booking_flag = false;
            //}
            //_context.SaveChanges();
            //var Invoices_items = _context.BookingMasters.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == false);
            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                ViewBag.Uname = uname;
                return View("Index");
            }
            else
            {
                // ViewBag.Message = "File upload failed!!";
                ViewBag.message = result.Message;
                return View("Failure");
            }
        }


    }

}
