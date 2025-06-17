//@model CoreLogin.Models.C_Registration



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cat_Id { get; set; }
        //public int L_Id { get; set; }

        [ForeignKey("Login")]
        public int? L_Id { get; set; }

        [Display(Name = "CategoryName")]
        public string Cat_Name { get; set; }
        public string Cat_Image { get; set; }
        //public double Cat_price { get; set; }


       
    }
}
