using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public List<Comment> Comment { get; set; }
        public UserProfile CommentUserProfile { get; set; }
        public Post CommentPost { get; set; }
    }
}
