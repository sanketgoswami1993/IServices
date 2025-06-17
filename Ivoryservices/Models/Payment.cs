using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int P_Id { get; set; }
        //public int  S_Id { get; set; }
        //public string User_name { get; set; }
        //public string Email { get; set; }
        //public int Quantity { get; set; }
        public double Totalprice { get; set; }
        // public string P_Type { get; set; }
        public DateTime DateTime { get; set; }
        //public string Mobile_no { get; set; }

    }
}
