using AspNetCoreGeneratedDocument;
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
        public IActionResult CreateProduct(Product productmodel)
        {
            if (ModelState.IsValid)
            {
                // ดู error
                _db.Add(productmodel);
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                Debug.WriteLine("Error create product -------------------------------" + errors.ToString());
                _db.SaveChanges();

                return RedirectToAction("Index","Cart");
            }
            return View();
        }


        
    }
}
