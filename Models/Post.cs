using System;
using System.Collections.Generic;

namespace UserDashboard.Models
{
    public class Post : BaseEntity
    {

        public int PostId { get; set; }
        
        public string Title { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public List<Comment> Comments { get; set; }
        
        public Post () {
            Comments = new List<Comment>();
        }
        
    }
}