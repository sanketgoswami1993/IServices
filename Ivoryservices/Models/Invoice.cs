using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int In_Id { get; set; }
        [ForeignKey("Logins")]
        //public int L_Id { get; set; }
        //[ForeignKey("SubCategories")]
        public int Sub_Id { get; set; }
        public string Sub_Name { get; set; }
        public string Sub_Price { get; set; }
        //public string UserName { get; set; }
        
        public double tax { get; set; }
        public double total { get; set; }
        public double grossTotal { get; set; }
        public DateTime Order_date { get; set; }

    }
}
