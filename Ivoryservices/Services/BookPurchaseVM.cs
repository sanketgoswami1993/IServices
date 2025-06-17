using Ivoryservices.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ivoryservices.Services
{
    [NotMapped]
    public class BookPurchaseVM : Payment
    {

        public string Nonce { get; set; }
    }
}
