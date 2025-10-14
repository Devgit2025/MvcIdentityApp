namespace MvcIdentityApp.Models
{
    public class CartItem
    {
        public int Cart_Id { get; set; }
        public string Cart_Name { get; set; }
        public decimal Cart_Price { get; set; }
        public int Cart_Quantity { get; set; }

        public decimal Cart_Total => Cart_Price * Cart_Quantity;
    }

}
