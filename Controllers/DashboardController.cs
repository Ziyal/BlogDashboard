using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserDashboard.Models;

namespace UserDashboard.Controllers
{
    public class DashboardController : Controller
    {

        private UserDashboardContext _context;
    
        public DashboardController(UserDashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {

            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.User = CurrentUser;

            ViewBag.AllUsers = _context.Users;

            return View("Dashboard");
        }

        [HttpGet]
        [Route("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id){

            // Remove Comments
            List<Comment> RemoveComments = _context.Comments.Where(user => user.UserId == id).ToList();
            foreach (var comment in RemoveComments) {
                _context.Remove(comment);
            }
            _context.SaveChanges();

            // Remove Posts
            List<Post> RemovePosts = _context.Posts.Where(user => user.UserId == id).ToList();
            foreach (var post in RemovePosts) {
                _context.Remove(post);
            }
            // Remove user
            User RemoveUser = _context.Users.Where(user => user.UserId == id).SingleOrDefault();
            _context.Remove(RemoveUser);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
        }

    }
}
