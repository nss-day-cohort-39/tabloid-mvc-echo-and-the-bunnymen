//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.VisualBasic;
//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using TabloidMVC.Models;
//using TabloidMVC.Models.ViewModels;
//using TabloidMVC.Repositories;

//namespace TabloidMVC.Controllers
//{
//    public class CommentController
//    {
//        private readonly CommentRepository _commentRepository;
//        private readonly UserProfileRepository _userProfileRepository;


//        public CommentController(IConfiguration config)
//        {
//            _commentRepository = new CommentRepository(config);
//            _userProfileRepository = new UserProfileRepository(config);
//        }

//        public IActionResult Index()
//        {
//            var comments = _commentRepository.GetAllComments();
//            return View(comments);
//        }

//        public IActionResult UserIndex()
//        {
//            int UserProfileId = GetCurrentUserProfileId();

//            List<Comment> userComment = _commentRepository.GetAllPostByUserId(UserProfileId);
//            return View(userComment);
//        }

//        public IActionResult Details(int id)
//        {
//            var post = _postRepository.GetPublisedPostById(id);
//            if (post == null)
//            {
//                int userId = GetCurrentUserProfileId();
//                post = _postRepository.GetUserPostById(id, userId);
//                if (post == null)
//                {
//                    return NotFound();
//                }
//            }
//            return View(post);
//        }

//        public IActionResult Create()
//        {
//            var vm = new PostCreateViewModel();
//            vm.CategoryOptions = _categoryRepository.GetAll();
//            return View(vm);
//        }

//        [HttpPost]
//        public IActionResult Create(PostCreateViewModel vm)
//        {
//            try
//            {
//                vm.Post.CreateDateTime = DateAndTime.Now;
//                vm.Post.IsApproved = true;
//                vm.Post.UserProfileId = GetCurrentUserProfileId();

//                _postRepository.Add(vm.Post);

//                return RedirectToAction("Details", new { id = vm.Post.Id });
//            }
//            catch
//            {
//                vm.CategoryOptions = _categoryRepository.GetAll();
//                return View(vm);
//            }
//        }

//        // GET: Posts/Edit/5
//        [Authorize]
//        public ActionResult Edit(int id)
//        {
//            int currentPostId = GetCurrentUserProfileId();
//            Post post = _postRepository.GetUserPostById(id, currentPostId);
//            List<Category> CategoryList = _categoryRepository.GetAll();
//            PostCreateViewModel vm = new PostCreateViewModel
//            {
//                Post = post,
//                CategoryOptions = CategoryList
//            };
//            if (vm.Post == null)
//            {
//                return NotFound();
//            }
//            if (currentPostId != vm.Post.UserProfileId)
//            {
//                return NotFound();
//            }
//            return View(vm);
//        }

//        // POST: Post/Edit/5
//        [Authorize]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(int id, PostCreateViewModel vm)
//        {
//            try
//            {
//                vm.CategoryOptions = _categoryRepository.GetAll();
//                vm.Post.Id = id;
//                vm.Post.CreateDateTime = DateTime.Now;
//                _postRepository.UpdatePost(vm.Post);

//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {
//                vm.CategoryOptions = _categoryRepository.GetAll();
//                return View(vm);
//            }
//        }

//        // GET: PostController/Delete/5
//        [Authorize]
//        public ActionResult Delete(int id)
//        {
//            int userId = GetCurrentUserProfileId();
//            Post post = _postRepository.GetUserPostById(id, userId);

//            return View(post);
//        }

//        // POST: PostController/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Delete(int id, Post post)
//        {
//            try
//            {
//                _postRepository.DeletePost(id);

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View(post);
//            }
//        }


//        private int GetCurrentUserProfileId()
//        {
//            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            return int.Parse(id);
//        }
//    }
//}
