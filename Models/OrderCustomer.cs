using System.ComponentModel.DataAnnotations;

namespace MvcIdentityApp.Models
{
    public class OrderCustomer
    {
        [Key]
        public  int Order_id { get; set; }
        public DateTime Order_date { get; set; }
        public string Order_name { get; set; }
        public string Order_email { get; set; }
        public string Order_tel { get; set; }
        public string Order_address { get; set; }
        public decimal Order_total { get; set; }

        public string ApplicationUse_id { get; set; }


        // Foreign Key
        //public int ApplicationUserId { get; set; }

        // Navigation Property
        //public ApplicationUser ApplicationUser { get; set; }

        //public ICollection<OrderDetail> OrderDetails { get; set; }




    }
}
