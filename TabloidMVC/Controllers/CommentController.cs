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
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UserProfileRepository _userProfileRepository;
        private readonly PostRepository _postRepository;


        public CommentController(IConfiguration config)
        {
            _commentRepository = new CommentRepository(config);
            _userProfileRepository = new UserProfileRepository(config);
            _postRepository = new PostRepository(config);
        }

        public IActionResult Index(int id)
        {
            CommentCreateViewModel vm = new CommentCreateViewModel();
            var comments = _commentRepository.GetCommentByPostId(id);
            vm.Comment = comments;
            Post postId = _postRepository.GetPublisedPostById(id);
            vm.CommentPost = postId;
            return View(vm);
        }

        public IActionResult UserIndex()
        {
            int UserProfileId = GetCurrentUserProfileId();

            List<Comment> userComment = _commentRepository.GetAllCommentsByUserId(UserProfileId);
            return View(userComment);
        }

        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Comment comment, int id)
        {
            try
            {
                comment.CreateDateTime = DateAndTime.Now;
                comment.UserProfileId = GetCurrentUserProfileId();
                comment.PostId = id;

                _commentRepository.Add(comment);

                return RedirectToAction("Index", new { id = comment.Id });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            int currentCommentId = GetCurrentUserProfileId();
            Comment comment = _commentRepository.GetUserCommentById(id, currentCommentId);
            
            if (comment == null)
            {
                return NotFound();
            }
            if (currentCommentId != comment.UserProfileId)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Post/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                comment.CreateDateTime = DateTime.Now;
                _commentRepository.UpdateComment(comment);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        // GET: PostController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            Comment comment = _commentRepository.GetUserCommentById(id, userId);

            return View(comment);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                _commentRepository.DeleteComment(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(comment);
            }
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
