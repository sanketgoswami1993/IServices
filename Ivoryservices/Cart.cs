
using Ivoryservices.Models;

namespace Ivoryservices
{
    public class Cart
    {
        public List<Item> items { get; set; }
        public double tax { get; set; }
        public double total { get; set; }

        public double grossTotal { get; set; }



    }
    public class RootObject
    {
        public List<Cart> data { get; set; }
    }
}
