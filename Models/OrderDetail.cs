using System.ComponentModel.DataAnnotations;

namespace MvcIdentityApp.Models
{
    public class OrderDetail
    {
        [Key]
        public int Order_id_detail { get; set; }

        // Foreign Key
        /*public int OrderCustomerId { get; set; }

        // Navigation Property
        public OrderCustomer OrderCustomer { get; set; }
        */
        
        // Foreign Key
        public int Order_id {get; set; }
        public int Pro_id { get; set; }
        public string Pro_name { get; set; }
        public decimal Pro_price { get; set; }
        
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        // Navigation Property
        //public Product Product { get; set; }







    }
}
