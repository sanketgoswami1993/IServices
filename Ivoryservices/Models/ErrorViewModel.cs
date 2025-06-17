using System.ComponentModel.DataAnnotations;

namespace Ivoryservices.Models
{
    public class ErrorViewModel
    {
        
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
    }
}