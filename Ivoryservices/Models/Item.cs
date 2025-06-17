using Microsoft.EntityFrameworkCore;

namespace Ivoryservices.Models
{
    [Keyless]
    public class Item   
    {
        public SubCategory? Product { get; set; }

        public int Quantity { get; set; }
        public double tax { get; set; }
        public double total { get; set; }

        public double grossTotal { get; set; }

    }
    //public class Price
    //{
    //    public List<Item> items { get; set; }
    //    //public double tax { get; set; }
    //    public double total { get; set; }

    //    public double grossTotal { get; set; }

    //}
}
