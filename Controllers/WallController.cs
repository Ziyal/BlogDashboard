using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDashboard.Models;

namespace UserDashboard.Controllers
{
    public class WallController : Controller
    {
        private UserDashboardContext _context;
    
        public WallController(UserDashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("blog/{id}")]
        public IActionResult Wall(int id){

            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.User = CurrentUser;

            ViewBag.Linked = _context.Users.SingleOrDefault(person => person.UserId == id);

            List<Post> AllPosts = _context.Posts.Where(post => post.UserId == id).Include(u => u.User).Include(p => p.Comments).ThenInclude(comment => comment.User).OrderByDescending(post => post.CreatedAt).ToList();
            ViewBag.UsersPosts = AllPosts;

            System.Console.WriteLine(AllPosts);

            
            return View("Wall");
        }


        [HttpPost]
        [Route("AddPost/{id}")]
        public IActionResult AddPost(Post model, int id){
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));

            Post NewPost = new Post {
                Title = model.Title,
                Content = model.Content,
                UserId = id
            };

            _context.Add(NewPost);
            _context.SaveChanges();
            
            return RedirectToAction("Wall");
        }


        [HttpPost]
        [Route("AddComment/{PostId}")]
        public IActionResult AddComment(Comment model, int PostId){
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));

            Post CurrentPost = _context.Posts.SingleOrDefault(post => post.PostId == PostId);

            Comment NewComment = new Comment {
                Content = model.Content,
                PostId = model.PostId,
                UserId = CurrentUser.UserId
            };

            _context.Add(NewComment);
            _context.SaveChanges();
            
            return RedirectToAction("Wall", new {id = CurrentPost.UserId});
        }


    }
}
