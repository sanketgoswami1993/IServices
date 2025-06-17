
using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class ServiceMaster
    {
        [Key] 
        public int SId { get; set; }
        public int L_Id { get; set; }
        public string SType { get; set; }

    }
}
