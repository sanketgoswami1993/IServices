using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Models
{
    public class SubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sub_Id { get; set; }

        [ForeignKey("Category")]
        public int? Cat_Id { get; set; }
        public string Sub_Name { get; set; }
        public double Sub_Price { get; set; }
        public string Sub_Image { get; set; }
    }
}
