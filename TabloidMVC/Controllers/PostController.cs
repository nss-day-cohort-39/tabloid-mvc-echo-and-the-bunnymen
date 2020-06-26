using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UserProfileRepository _userProfileRepository;
        private readonly TagRepository _tagRepository;


        public PostController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _categoryRepository = new CategoryRepository(config);
            _userProfileRepository = new UserProfileRepository(config);
            _tagRepository = new TagRepository(config);
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult UserIndex()
        {
            int UserProfileId = GetCurrentUserProfileId();

            List<Post> userPost = _postRepository.GetAllPostByUserId(UserProfileId);
            return View(userPost);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublisedPostById(id);
            var postTags = _tagRepository.GetPostTagsByPostId(id);
            var tags = new List<Tag>();
            foreach (PostTag pt in postTags)
            {
                Tag tag = _tagRepository.GetTagById(pt.TagId);
                tags.Add(tag);
            }
            PostTagViewModel vm = new PostTagViewModel
            {
                Post = post,
                PostTagList = postTags,
                TagOptions = tags
            };
            
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true; 
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            int currentPostId = GetCurrentUserProfileId();
            Post post = _postRepository.GetUserPostById(id, currentPostId);
            List<Category> CategoryList = _categoryRepository.GetAll();
            PostCreateViewModel vm = new PostCreateViewModel
            {
                Post = post,
                CategoryOptions = CategoryList
            };
            if (vm.Post == null)
            {
                return NotFound();
            }
            if (currentPostId != vm.Post.UserProfileId)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: Post/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                vm.Post.Id = id;
                vm.Post.CreateDateTime = DateTime.Now;
                _postRepository.UpdatePost(vm.Post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: PostController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetUserPostById(id, userId);

            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }



        public ActionResult Add(int id)
        {
            Post post = _postRepository.GetPublisedPostById(id);
            List<Tag> tags = _tagRepository.GetAll();

            PostTagViewModel vm = new PostTagViewModel();
            vm.TagOptions = tags;
            vm.Post = post;
            vm.PostTag = new PostTag();
            vm.PostTag.PostId = post.Id;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(PostTagViewModel vm, int id)
        {
            try
            {
            int userId = GetCurrentUserProfileId();
                vm.PostTag.PostId = id;
            
            //Post post = _postRepository.GetUserPostById(id, userId);
            _tagRepository.InsertTag(vm.PostTag);

                return RedirectToAction("Details", new { id = id });

            }
            catch
            {
                //List<Tag> tags = _tagRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
