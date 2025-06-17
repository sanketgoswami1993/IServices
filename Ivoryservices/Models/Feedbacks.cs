using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class Feedbacks
    {
        [Key]
        public int F_Id { get; set; }
        public int L_Id { get; set; }
        public string Description { get; set; }
        public DateTime F_Date { get; set; }

    }
}
