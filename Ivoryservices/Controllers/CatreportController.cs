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
    public class CatreportController : Controller
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
        public CatreportController(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment, ILogger<HomeController> logger, OnestopContext context)
        {
            Environment = _environment;
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();

        }
        public FileResult CatPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(2);
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

            float[] headers = { 50, 50};  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 95;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top

            // List<BookingMaster>? Bookingmaster = _context.BookingMasters.ToList<BookingMaster>();
            // _context.BookingMasters.
            var Invoices_items = _context.Categories.ToList();

            //logo
            PdfPCell cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA)));
            { cell.BorderWidth = 0; cell.Colspan = 12; cell.FixedHeight = 10f; }
            tableLayout.AddCell(cell);

            PdfPCell cell2 = new PdfPCell(new Phrase("One-Stop Solutions",
                new Font(Font.FontFamily.HELVETICA, 20, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 12, Border = 0 };
            cell2.Colspan = 12;

            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            tableLayout.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase("Date" + DateTime.Now.ToShortDateString(),
                new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 12, Border = 0 };
            cell3.Colspan = 12;

            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableLayout.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase("Time" + DateTime.Now.ToShortTimeString(),
               new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            { Colspan = 12, Border = 0 };
            cell4.Colspan = 12;

            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableLayout.AddCell(cell4);

            PdfPCell WhitheSpace = new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA)));
            { WhitheSpace.BorderWidth = 0; WhitheSpace.Colspan =12; WhitheSpace.FixedHeight = 10f; }
            tableLayout.AddCell(WhitheSpace);
            // tableLayout.AddCell(new PdfPCell(new Phrase("One-Stop Solutions", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            // tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            // tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header
           
            AddCellToHeader(tableLayout, "Category Name");
            AddCellToHeader(tableLayout, "Category Image");
            ////Add body




            foreach (var bkm in Invoices_items)
            {

                
                AddCellToBody(tableLayout, bkm.Cat_Name.ToString());
                AddCellToBody(tableLayout, bkm.Cat_Image.ToString());
                


            }

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