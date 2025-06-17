using Microsoft.EntityFrameworkCore;
using Ivoryservices.Models;
using Ivoryservices.Services;
//using DocumentFormat.OpenXml.Office.CustomUI;

namespace Ivoryservices.Models
{
    public class OnestopContext:DbContext
    {
        
        public OnestopContext(DbContextOptions<OnestopContext> options):base(options)
        {
            
        }
        public virtual DbSet<Login> Logins { get; set; }
        
        public virtual DbSet<C_Registration>C_Registrations  { get; set; }
        public virtual DbSet<ServiceMaster> ServiceMasters { get; set; }
        public virtual DbSet<Category> CategoryMasters { get; set; }
        public virtual DbSet<Order_service> Order_Services { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
       // public virtual DbSet<Admin> AdminMasters { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Gallery> Gallery { get; set; } 
        public virtual DbSet<Invoice> Invoices { get; set; }    
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<BookingMaster> BookingMasters { get; set; }
        public DbSet<Ivoryservices.Models.user> user { get; set; }
        public DbSet<Item> items { get; set; }
        public DbSet<Ivoryservices.Services.BookPurchaseVM> BookPurchaseVM { get; set; }
        // public DbSet<Ivoryservices.Models.Profile>? Profile { get; set; }
        //public object Order_service { get; internal set; }
    }

    
    
}
