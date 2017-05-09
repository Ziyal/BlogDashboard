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
    public class EditController : Controller
    {

        private UserDashboardContext _context;
    
        public EditController(UserDashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("profile/edit/{id}")]
        public IActionResult EditUser(int id) {
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.User = CurrentUser;
            ViewBag.EditUser = _context.Users.SingleOrDefault(person => person.UserId == id);
            ViewBag.Errors = TempData["Errors"];
            return View("EditUser");
        }

        [HttpPost]
        [Route("UpdateUserA/{id}")]
        public IActionResult UpdateInfoA(int id, User model) {
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == id);
            ViewBag.User = CurrentUser;

            System.Console.WriteLine(model.AccessLevel);

            CurrentUser.FirstName = model.FirstName;
            CurrentUser.LastName = model.LastName;
            CurrentUser.Email = model.Email;
            CurrentUser.AccessLevel = model.AccessLevel;
            CurrentUser.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            TempData["Success"] = "User info successfuly updated";
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpPost]
        [Route("UpdatePassword/{id}")]
        public IActionResult UpdatePassword(int id, string Password, String PasswordC) {
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == id);
            ViewBag.User = CurrentUser;

            var Hasher = new PasswordHasher<User>();
            // Check if passwords match then add to DB
            if (Password == PasswordC) {     
                CurrentUser.Password = Password;
                CurrentUser.Password = Hasher.HashPassword(CurrentUser, CurrentUser.Password);
                CurrentUser.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                TempData["Success"] = "Passwords successfuly changed";
                return RedirectToAction("EditUser");
            }
            // If password match fails
            TempData["Errors"] = "Passwords do not match";
            return RedirectToAction("EditUser");
        }   

        [HttpPost]
        [Route("UpdateDescription/{id}")]
        public IActionResult UpdateDescription(int id, string Description) {
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == id);
            ViewBag.User = CurrentUser;

            if (CurrentUser.Description == null) {
                _context.Add(CurrentUser.Description = Description);
                _context.SaveChanges();
            }
            CurrentUser.Description = Description;
            _context.SaveChanges();
            TempData["Success"] = "Description successfuly changed";
            return RedirectToAction("EditUser");
        }         
    }
}
