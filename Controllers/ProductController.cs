using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcIdentityApp.Data;
using MvcIdentityApp.Models;
using System.Diagnostics;

namespace MvcIdentityApp.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct() 
        { 
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product productmodel, IFormFile ImageFile)
        {
            //if (ModelState.IsValid)
            //{
                //  อัพโหลดภาพ
                Debug.WriteLine("------------------------ ImageFile : " + ImageFile.FileName);
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // ตั้งชื่อไฟล์ไม่ให้ซ้ำ
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    string newFileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                    // กำหนดโฟลเดอร์เก็บรูป (wwwroot/images)
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // ถ้าไม่มีโฟลเดอร์ ให้สร้างใหม่
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    // path เต็มของไฟล์ที่จะเก็บ
                    string filePath = Path.Combine(uploadPath, newFileName);

                    // เซฟไฟล์ลงโฟลเดอร์
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // เก็บ path ลง database
                    productmodel.Pro_Img = "/images/" + newFileName;
                }


                // บันทึกข้อมูลลง database
                // ดู error
                _db.Add(productmodel);
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                Debug.WriteLine("Error create product -------------------------------" + errors.ToString());
                _db.SaveChanges();
                
                return RedirectToAction("Index","Cart");
            //}
            //return View();
        }

        public IActionResult EditProduct()
        {
            return View();
        }
        
    }
}
