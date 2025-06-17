using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class user
    {
        [Key]
        public int Cat_Id { get; set; }
        public string Cat_Name { get; set; }
    }
}
