using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Models
{
    public class Admin
    {
        [Key]
        public int A_Id { get; set; }
        [ForeignKey("Login")]
        public int? L_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImg { get; set; }
        //public string Roles { get; set; }

    }
}
