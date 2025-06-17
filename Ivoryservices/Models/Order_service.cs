using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ivoryservices.Models
{
    public class Order_service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_Id { get; set; }
        public int L_Id { get; set; }
        public string Description  { get; set; }
        public string Address  { get; set; }
        public string City  { get; set; }

        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public Int64 Mobile { get; set; }
        public DateTime BookDate { get; set; }
        public bool Status  { get; set; }
    }
}
