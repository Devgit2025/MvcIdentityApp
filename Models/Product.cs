using System.ComponentModel.DataAnnotations;

namespace MvcIdentityApp.Models
{
    public class Product
    {
        [Key]
        public int Pro_Id { get; set; }

        [Required]
        public string Pro_Name { get; set; }

        [Required]
        public decimal Pro_Price { get; set; }

        [Required]
        public string Pro_Detail { get; set; }

        [Required]
        public String Pro_Img { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "กรุณาป้อนสินค้าในคลัง 0-1000")]
        public int Pro_Stock { get; set; }
    }
}
