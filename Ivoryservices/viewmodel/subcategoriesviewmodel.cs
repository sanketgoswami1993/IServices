using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.viewmodel
{
    public class subcategoriesviewmodel
    {
        public int Sub_Id { get; set; }
        [ForeignKey("Category")]
        public int? Cat_Id { get; set; }
        [Display(Name = "CategoryName")]
        public string Cat_Name { get; set; }
        [Display(Name = "SubCategoryName")]
        public string Sub_Name { get; set; }
        [Display(Name = "SubCategoryPrice")]
        public float Sub_Price { get; set; }
        [Display(Name = "SubCategoryImage")]
        public  IFormFile Sub_Image { get; set; }
    }
}
