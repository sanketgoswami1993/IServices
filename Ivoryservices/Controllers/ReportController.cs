using DocumentFormat.OpenXml.Wordprocessing;
using Grpc.Core;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Ivoryservices.Models;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using CrystalDecisions.ReportAppServer.DataDefModel;
using DocumentFormat.OpenXml.Presentation;
using iText.IO.Font.Constants;

namespace Ivoryservices.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnestopContext _context;

        public IWebHostEnvironment WebHostEnvironment { get; }

        //public ReportController(ILogger<HomeController> logger, OnestopContext context, IWebHostEnvironment webHostEnvironment)
        //{
        //    _logger = logger;
        //    _context = context;
        //   //WebHostEnvironment = webHostEnvironment;
        //}
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        public ReportController(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment, ILogger<HomeController> logger, OnestopContext context)
        {
            Environment = _environment;
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();

        }
        public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(7);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string strAttachment = Path.Combine(this.Environment.WebRootPath, "Downloadss");
            //file will created in this path
            // string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 30, 45, 35, 50, 50, 50 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 95;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top

            // List<BookingMaster>? Bookingmaster = _context.BookingMasters.ToList<BookingMaster>();
            // _context.BookingMasters.
            var Invoices_items = _context.BookingMasters
                .Where(x => x.UserName == HttpContext.Session.GetString("User_name").ToString() && x.booking_flag == true)
                .ToList();
            // Invoices_items.Where(x => x.L_Id == Convert.ToInt16(TempData["loginid"]) && 
            //logo
            PdfPCell cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA)));
            { cell.BorderWidth = 0; cell.Colspan = 12; cell.FixedHeight = 10f; }
            tableLayout.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase("Invoice",
               new Font(Font.FontFamily.HELVETICA, 20, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 12, Border = 0 };
            cell1.Colspan = 12;

            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            tableLayout.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase("One-Stop Solutions",
                new Font(Font.FontFamily.HELVETICA, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 6, Border = 0 };
            cell2.Colspan = 6;

            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            tableLayout.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase("Date" + DateTime.Now.ToShortDateString(),
                new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 6, Border = 0 };
            cell3.Colspan = 6;

            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableLayout.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase("Time" + DateTime.Now.ToShortTimeString(),
               new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 8, Border = 0 };
            cell4.Colspan = 8;

            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableLayout.AddCell(cell4);

            PdfPCell WhitheSpace = new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA)));
            { WhitheSpace.BorderWidth = 0; WhitheSpace.Colspan = 12; WhitheSpace.FixedHeight = 10f; }
            tableLayout.AddCell(WhitheSpace);
            // tableLayout.AddCell(new PdfPCell(new Phrase("One-Stop Solutions", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            // tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            // tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header
            AddCellToHeader(tableLayout, "UserName");
            AddCellToHeader(tableLayout, "SubCategory Name");
            AddCellToHeader(tableLayout, "SubCategory Price");
            AddCellToHeader(tableLayout, "Tax");
            AddCellToHeader(tableLayout, "Quantity");
            AddCellToHeader(tableLayout, "Order Date");
            AddCellToHeader(tableLayout, "Total");
            ////Add body




            foreach (var bkm in Invoices_items)
            {

                AddCellToBody(tableLayout, bkm.UserName.ToString());
                AddCellToBody(tableLayout, bkm.Sub_Name.ToString());
                AddCellToBody(tableLayout, bkm.Sub_Price.ToString());
                AddCellToBody(tableLayout, bkm.tax.ToString());
                AddCellToBody(tableLayout, bkm.Quantity.ToString());
                AddCellToBody(tableLayout, bkm.Order_date.ToString());
                AddCellToBody(tableLayout, bkm.total.ToString());


            }

            PdfPCell cells = new PdfPCell(new Phrase("",
               new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 3, Border = 0 };
            cells.Colspan = 3;

            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            tableLayout.AddCell(cells);
            string amt=  String.Format("GrandTotal : {0:#,##0}", Convert.ToDouble(HttpContext.Session.GetString("Tot_amt")));
            PdfPCell cell5 = new PdfPCell(new Phrase(amt,
               new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 8 , Border = 0 };
            cell5.Colspan = 8;

            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            tableLayout.AddCell(cell5);

            string uname = HttpContext.Session.GetString("User_name").ToString();
            //string loginid= HttpContext.Session.GetString("login_id").ToString();
            var query = _context.BookingMasters.Where(x => x.UserName == uname).ToList();
            foreach (var b in query)
            {
                b.booking_flag = false;
            }
            _context.SaveChanges();

            return tableLayout;
        }

        private void AddCellToBody(PdfPTable tableLayout, DateTime order_date)
        {
            throw new NotImplementedException();
        }

        private void AddCellToBody(PdfPTable tableLayout, double sub_Price)
        {
            throw new NotImplementedException();
        }

        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }
        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        }
        public IActionResult Booking_byDate()
        {
            return View();
        }
    }

}