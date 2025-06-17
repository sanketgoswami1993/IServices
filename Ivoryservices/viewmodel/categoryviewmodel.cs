using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.viewmodel
{
    public class categoryviewmodel
    {
        public int Cat_Id { get; set; }
        [Display(Name = "CategoryName")]
        public string Cat_Name { get; set; }
        [Display(Name = "CategoryImage")]
        public IFormFile Cat_Image { get; set; }
        [Display(Name = "CategoryPrice")]
        public float Cat_price { get; set; }

    }

}
