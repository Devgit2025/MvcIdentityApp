using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcIdentityApp.Models
{
    public class ApplicationUser:IdentityUser
    {

        // เพิ่มฟิลด์ใหม่ได้ เช่น
        public required string FullName { get; set; }
        public DateTime CreatedAt { get; set; }

        // 1 Category มีได้หลาย Product
        //public ICollection<OrderCustomer> OrderCustomers { get; set; }


    }
}
