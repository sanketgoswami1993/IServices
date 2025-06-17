using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class Login
    {
        [Key]
        public int L_Id { get; set; }
        public int Regis_Id { get; set; } 
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }

        public string Roles { get; set; }

    }
}