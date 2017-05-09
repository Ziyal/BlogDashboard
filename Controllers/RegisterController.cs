using System;
using System.Collections.Generic;
using System.Linq;
using UserDashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.Controllers
{
    public class RegisterController : Controller {
        private UserDashboardContext _context;
    
        public RegisterController(UserDashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Index(){
            ViewBag.Errors = TempData["Errors"];
            return View("Register");
        }

        [HttpPost]
        [Route("register_user")]
        public IActionResult RegisterUser(User model)  {
            List<string> allErrors = new List <string>();

            if(ModelState.IsValid) {
                User CheckUser = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                
                if (CheckUser != null) {
                    allErrors.Add("Email already in use");
                    TempData["Errors"] = allErrors;
                    return RedirectToAction("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User newUser = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    AccessLevel = model.AccessLevel,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                
                _context.Add(newUser);
                _context.SaveChanges();
                // Grab user id
                User user = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                HttpContext.Session.SetInt32("CurrUserId", user.UserId);

                return RedirectToAction("Success");
            }
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("Index", model);
        }

        [HttpGet]
        [Route("register-success")]
        public IActionResult Success()
        {
            return RedirectToAction("Dashboard", "Dashboard");
        }        

    }
}
