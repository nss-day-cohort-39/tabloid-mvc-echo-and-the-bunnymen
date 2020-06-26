using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }
        public Tag tag { get; set; }
        public PostTag PostTag { get; set; }
        public List<Tag> TagOptions { get; set; }
        public List<PostTag> PostTagList { get; set; }
    }
}
