using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcIdentityApp.Data;
using MvcIdentityApp.Models;
using Newtonsoft.Json;
using System.Diagnostics; // ต้องใช้ NuGet: Microsoft.AspNetCore.Session + Newtonsoft.Json
using System.Linq;


namespace MvcIdentityApp.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db; // ✅ inject จาก DI container
        }
        

        //private readonly ApplicationDbContext db = new ApplicationDbContext();


        // 🔹 จำลองสินค้า (ในโปรเจกต์จริงจะดึงจากฐานข้อมูล)
        /*private List<Product> products = new List<Product>()
        {
        new Product{ Pro_Id = 1, Pro_Name = "เสื้อยืด", Pro_Price = 250 },
        new Product{ Pro_Id = 2, Pro_Name = "กางเกงยีนส์", Pro_Price = 750 },
        new Product{ Pro_Id = 3, Pro_Name = "รองเท้า", Pro_Price = 1200 }
        };*/

        // แสดงสินค้าทั้งหมด
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserName");

            /*if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");

            }*/
            ViewBag.Username = username;
            // ดึงข้อมูลผู้ใช้ทั้งหมด
            //var users = _context.Users.ToList();  // ✅ จาก IdentityDbContext
            var products = _db.Products.ToList();
            return View(products);
        }

        // เพิ่มสินค้าเข้าตะกร้า
        public IActionResult AddToCart(int id)
        {
            
            var product = _db.Products.FirstOrDefault(p => p.Pro_Id == id);
            if (product == null) return NotFound();

            var cart = GetCart();

            var item = cart.FirstOrDefault(c => c.Cart_Id == id);
            if (item != null)
                item.Cart_Quantity++;
            else
                cart.Add(new CartItem
                {
                    Cart_Id = product.Pro_Id,
                    Cart_Name = product.Pro_Name,
                    Cart_Price = product.Pro_Price,
                    Cart_Quantity = 1
                });

            SaveCart(cart);

            return RedirectToAction("Cart");
        }


        // แสดงตะกร้าสินค้า
        public IActionResult Cart()
        {
            var username = HttpContext.Session.GetString("UserName");

            /*if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");

            }*/
            ViewBag.Username = username;

            var cart = GetCart();
            return View(cart);
        }


        // ลบสินค้าออกจากตะกร้า
        public IActionResult RemoveItem(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.Cart_Id == id);
            if (item != null)
                cart.Remove(item);

            SaveCart(cart);
            return RedirectToAction("Cart");
        }


        // ล้างตะกร้าทั้งหมด
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Cart");
        }

        // 🔹 อ่านข้อมูลจาก Session
        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            if (sessionData == null)
            {
                return new List<CartItem>();
            }

            return JsonConvert.DeserializeObject<List<CartItem>>(sessionData);
        }


        // 🔹 บันทึกข้อมูลลง Session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

    }
}
