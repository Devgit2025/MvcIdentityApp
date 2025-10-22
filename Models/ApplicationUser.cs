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



        /*[Required(ErrorMessage = "กรุณาป้อนชื่อเต็ม")]
        [DisplayName("Fullname")]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "กรุณาป้อนเบอร์โทรศัพท์")]
        [DisplayName("Tel.")]
        public string Tel { get; set; }
        [Required(ErrorMessage = "กรุณาป้อนที่อยู่")]
        [DisplayName("Address")]
        public string Address { get; set; }
        */


        //[Required(ErrorMessage = "กรุณาป้อนชื่อเต็ม")]
        //[DisplayName("Fullname")]
        //public required string Fullname { get; set; }
        /*
        [Required(ErrorMessage = "กรุณาป้อนเบอร์โทรศัพท์")]
        [DisplayName("Tel")]
        public string Tel { get; set; }
        */

        /*[Required(ErrorMessage = "กรุณาป้อนที่อยู่")]
        [DisplayName("Address")]
        public string Address { get; set; }
        */

    }
}
