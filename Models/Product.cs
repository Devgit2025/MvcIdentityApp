using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcIdentityApp.Models
{
    public class Product
    {
        [Key]
        public int Pro_Id { get; set; }

        [Required(ErrorMessage = "กรุณาป้อนชื่อสินค้า")]
        [DisplayName("ชื่อสินค้า")]
        public string Pro_Name { get; set; }

        [Required(ErrorMessage = "กรุณาป้อนราคาสินค้า")]
        [DisplayName("ราคาสินค้า")]
        public decimal Pro_Price { get; set; }

        [Required(ErrorMessage = "กรุณาป้อนรายละเอียดสินค้า")]
        [DisplayName("รายละเอียดสินค้า")]
        public string Pro_Detail { get; set; }

        [DisplayName("ภาพสินค้า")]
        public String Pro_Img { get; set; } // เก็บ path ของรูปภาพ

        [Required]
        [DisplayName("จำนวนสินค้า")]
        [Range(0, 1000, ErrorMessage = "กรุณาป้อนสินค้าในคลัง 0-1000")]
        public int Pro_Stock { get; set; }
    }
}
