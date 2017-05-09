using System;
using System.Collections.Generic;
using System.Linq;
using UserDashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.Controllers
{
    public class LoginController : Controller {
        private UserDashboardContext _context;
    
        public LoginController(UserDashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Index()
        {
            ViewBag.Errors = TempData["Errors"];
            return View("Login");
        }

        [HttpGet]
        [Route("login_page")]
        public IActionResult Login()
        {
            ViewBag.Errors = TempData["Errors"];
            return View("Login");
        }

        [HttpPost]
        [Route("login_user")]
        public IActionResult Login(User model)  {
            List<string> allErrors = new List <string>();

            if(ModelState.IsValid) {
                User user = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                System.Console.WriteLine(user);
                // Check if user exists
                if (user != null && model.Password != null) {
                
                    var Hasher = new PasswordHasher<User>();
                    // Check for correct password
                    if (0 != Hasher.VerifyHashedPassword(user, user.Password, model.Password)) {
                        HttpContext.Session.SetInt32("CurrUserId", user.UserId);
                        return RedirectToAction("Success");
                    }
                    else {
                        allErrors.Add("Incorrect password");
                        TempData["Errors"] = allErrors;
                    }
                }
            }
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("login-success")]
        public IActionResult Success()
        {
            return RedirectToAction("Dashboard", "Dashboard");
        }        

    }
}
