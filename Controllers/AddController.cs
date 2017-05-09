using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserDashboard.Models;

namespace UserDashboard.Controllers
{
    public class AddController : Controller
    {

        private UserDashboardContext _context;
    
        public AddController(UserDashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("user/new")]
        public IActionResult AddNewUser()
        {

            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.User = CurrentUser;

            ViewBag.AllUsers = _context.Users;
            ViewBag.Errors = TempData["Errors"];
            ViewBag.Success = TempData["Success"];

            return View("AddUser");
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(UserViewModel modelV, User model)  {
            List<string> allErrors = new List <string>();

            if(ModelState.IsValid) {
                User CheckUser = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                
                if (CheckUser != null) {
                    allErrors.Add("Email already in use");
                    TempData["Errors"] = allErrors;
                    return RedirectToAction("AddNewUser");
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
                TempData["Success"] = "User added";
                return RedirectToAction("AddNewUser");
            }
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("AddNewUser");
        }
    }
}
