using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class Offer
    {
        [Key]
        public int Offer_Id { get; set; }
        public int L_Id { get; set; }
        public float Discount { get; set; }
    }
}
