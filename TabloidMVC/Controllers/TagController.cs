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
    public class TagController : Controller
    {
        private readonly TagRepository _tagRepository;



        public TagController(IConfiguration config)
        {
            _tagRepository = new TagRepository(config);
        }

        public IActionResult Index()
        {
            var tags = _tagRepository.GetAll();
            return View(tags);
        }

        //public IActionResult UserIndex()
        //{
        //    int UserProfileId = GetCurrentUserProfileId();

        //    List<Tag> userTag = _tagRepository.GetAllTagByUserId(UserProfileId);
        //    return View(userTag);
        //}

        //public IActionResult Details(int id)
        //{
        //    var tag = _tagRepository.GetPublisedTagById(id);
        //    if (tag == null)
        //    {
        //        int userId = GetCurrentUserProfileId();
        //        tag = _tagRepository.GetUserTagById(id, userId);
        //        if (tag == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    return View(tag);
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.Add(tag);

                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: Tags/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);  
            
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tag/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: Tag/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);

            return View(tag);
        }

        // POST: Tag/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

