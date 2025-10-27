using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcIdentityApp.Data;
using MvcIdentityApp.Models;
using System.Diagnostics;

namespace MvcIdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }

        

        // 🟩 REGISTER
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string username, string email, string password, string phonenumber)
        {
            //var user = new ApplicationUser { UserName = username, Email = email, FullName = fullName };
            //Debug.WriteLine("Username = "+user.UserName+" Email = "+user.Email+" Fullname = "+user.FullName);
            //Debug.WriteLine("Password = "+password);
            var user = new ApplicationUser
            {
                FullName = fullName,
                UserName = username,
                Email = email,
                PhoneNumber = phonenumber
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // ตรวจสอบว่ามี Role “User” หรือยัง
                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        // 🟩 LOGIN
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            if (username != null && password != null)
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                if (result.Succeeded)
                {
                    var fullname_user = _db.Users.FirstOrDefault(u => u.UserName == username);
                    Debug.WriteLine("====================================== Fullname : " + fullname_user.FullName);

                    
                    HttpContext.Session.SetString("Fullname", fullname_user.FullName);
                    HttpContext.Session.SetString("Email", fullname_user.Email);
                    HttpContext.Session.SetString("Phone", fullname_user.PhoneNumber);

                    HttpContext.Session.SetString("UserName", username);
                    HttpContext.Session.SetString("User_Id", fullname_user.Id);
                    return RedirectToAction("Index", "Cart");
                }
            }

            
            ViewBag.Error = "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
            return View();
        }

        public IActionResult Dashboard()
        {

            var username = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");

            }
            ViewBag.Username = username;
            return View();
        }


        // 🟩 LOGOUT
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

    }
}
