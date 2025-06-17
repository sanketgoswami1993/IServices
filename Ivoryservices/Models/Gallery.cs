using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class Gallery
    {
        [Key]
        public int G_Id { get; set; }
        public int Cat_Id { get; set; }
        public string G_Image { get; set; }
        public string G_Description { get; set; }

    }
}