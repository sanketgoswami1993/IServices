using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Models
{
    public class BookingMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int BId { get; set; }
        //[ForeignKey("Category")]
        //public int? Cat_Id { get; set; }
        ////[ForeignKey("SubCategories")]
        ////public int? Sub_Id { get; set; }
        [ForeignKey("Login")]
        public int? L_Id { get; set; }

        // public SubCategory Product { get; set; }

        public int Quantity { get; set; }
        public string Sub_Name { get; set; }

        public double Sub_Price { get; set; }
        public double tax { get; set; }
        public double total { get; set; }
        public double grossTotal { get; set; }
        public DateTime Order_date { get; set; }
        public string UserName { get; set; }

        public bool booking_flag { get; set; }
        //public object TimeUtc { get; internal set; }
        //public string ServiceProvider { get; set; }
    }
}
