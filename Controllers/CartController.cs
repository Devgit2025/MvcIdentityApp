using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MvcIdentityApp.Data;
using MvcIdentityApp.Models;
using Newtonsoft.Json;
using System.Diagnostics; // ต้องใช้ NuGet: Microsoft.AspNetCore.Session + Newtonsoft.Json
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace MvcIdentityApp.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _db;

        public object Session { get; private set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db; // ✅ inject จาก DI container
        }

        

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

        public IActionResult OrderBuy()
        {
            var username = HttpContext.Session.GetString("UserName");
            var fullname = HttpContext.Session.GetString("Fullname");
            var email = HttpContext.Session.GetString("Email");
            var phone = HttpContext.Session.GetString("Phone");


            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");

            }
            ViewBag.Username = username;
            ViewBag.Fullname = fullname;
            ViewBag.Email = email;
            ViewBag.Phone = phone;

            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Checkout(string fullname, string email, string phone, string address) 
        {
            fullname = HttpContext.Session.GetString("Fullname");

            HttpContext.Session.SetString("Email", email);
            email = HttpContext.Session.GetString("Email");

            HttpContext.Session.SetString("Phone", phone);
            phone = HttpContext.Session.GetString("Phone");

            HttpContext.Session.SetString("Address", address);
            address = HttpContext.Session.GetString("Address");

            var username = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");

            }
            ViewBag.Username = username;
            ViewBag.Fullname = fullname;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            ViewBag.Address = address;

            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculator(decimal total_price, int[] pro_id, string[] pro_name,decimal[] pro_price, int[] quantity, decimal[] total)
        {
            var username = HttpContext.Session.GetString("UserName");
            /*if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");

            }*/
            // ✅ รับค่าจาก hidden และ textbox ได้ตรงๆ
            Debug.WriteLine("----------------------Total : "+total_price);   // 123

            HttpContext.Session.Remove("Cart");

            //get db from ApplicationUser
            // ✅ ดึงข้อมูลจาก DB โดยมีเงื่อนไข username
            Debug.WriteLine("----------------------- Username : "+ username);

            var user = _db.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                Debug.WriteLine("----------------------- User Null");

                //return NotFoundResult();
            }
            Debug.WriteLine("----------------------- User ID : " +user.Id);

            //insert to db
            // 1️⃣ สร้างออเดอร์ใหม่
            var order_customer = new OrderCustomer
            {
                Order_date = DateTime.Now,
                Order_name = HttpContext.Session.GetString("Fullname"),
                Order_email = HttpContext.Session.GetString("Email"),
                Order_tel = HttpContext.Session.GetString("Phone"),
                Order_address = HttpContext.Session.GetString("Address"),
                Order_total = total_price,
                ApplicationUse_id = user.Id
                
            };

            // 2️⃣ บันทึกลงฐานข้อมูลก่อน เพื่อให้ได้ OrderId
            _db.OrderCustomers.Add(order_customer);
            await _db.SaveChangesAsync();
            

            // ตอนนี้ order.OrderId มีค่าแล้ว

            // 3️⃣ ใช้ OrderId ใส่ใน OrderDetail
            for (int i=0;i<pro_id.Length;i++)
            {
                Debug.WriteLine("----------------------- Pro_id : " + pro_id[i]);
                Debug.WriteLine("======================= Order_id : " + order_customer.Order_id);
                Debug.WriteLine("Pro id lenght : " + i.ToString());
                var order_dedtail = new OrderDetail
                {
                    Order_id = order_customer.Order_id,   // ใช้ id ที่เพิ่งได้มา
                    Pro_id = pro_id[i],
                    Pro_name = pro_name[i],
                    Pro_price = pro_price[i],
                    Quantity = quantity[i],
                    Total = total[i]

                };
                _db.OrderDetails.Add(order_dedtail);

                // 4️⃣ บันทึกข้อมูลลงฐานข้อมูลอีกครั้ง
                await _db.SaveChangesAsync();
            }
            return Ok("บันทึกข้อมูลสำเร็จ");
            
        }

        public IActionResult Orderdetail()
        {
            var username = HttpContext.Session.GetString("UserName");
            ViewBag.Username = username;

            var user_id = HttpContext.Session.GetString("User_Id");
            Debug.WriteLine("--------------------- User ID  : " + user_id);

            var order_customer = _db.OrderCustomers.Where(c => c.ApplicationUse_id == user_id).ToList();

            if (order_customer == null) 
            {
                return View();
            }
            return View(order_customer);
        }

        public IActionResult Detail(int id)
        {
            var username = HttpContext.Session.GetString("UserName");
            ViewBag.Username = username;

            var fullname = HttpContext.Session.GetString("Fullname");
            var email = HttpContext.Session.GetString("Email");
            var phone = HttpContext.Session.GetString("Phone");
            var address = HttpContext.Session.GetString("Address");
            ViewBag.Fullname = fullname;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            ViewBag.Address = address;

            //var order_detail = _db.OrderDetails.FirstOrDefault(d => d.Order_id == id);
            var order_detail = _db.OrderDetails.Where(d => d.Order_id == id).ToList();
            
            //Debug.WriteLine("------------------ Proname : "+ order_detail.Pro_name);

            return View(order_detail);
        }
    }
}
