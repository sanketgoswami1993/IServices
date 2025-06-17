using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int L_Id { get; set; }
    }
}
