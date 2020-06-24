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

        //public IActionResult Create()
        //{
        //    var vm = new TagCreateViewModel();
        //    vm.CategoryOptions = _categoryRepository.GetAll();
        //    return View(vm);
        //}

        //[HttpTag]
        //public IActionResult Create(TagCreateViewModel vm)
        //{
        //    try
        //    {
        //        vm.Tag.CreateDateTime = DateAndTime.Now;
        //        vm.Tag.IsApproved = true;
        //        vm.Tag.UserProfileId = GetCurrentUserProfileId();

        //        _tagRepository.Add(vm.Tag);

        //        return RedirectToAction("Details", new { id = vm.Tag.Id });
        //    }
        //    catch
        //    {
        //        vm.CategoryOptions = _categoryRepository.GetAll();
        //        return View(vm);
        //    }
        //}

        //// GET: Tags/Edit/5
        //[Authorize]
        //public ActionResult Edit(int id)
        //{
        //    int currentTagId = GetCurrentUserProfileId();
        //    Tag tag = _tagRepository.GetUserTagById(id, currentTagId);
        //    List<Category> CategoryList = _categoryRepository.GetAll();
        //    TagCreateViewModel vm = new TagCreateViewModel
        //    {
        //        Tag = tag,
        //        CategoryOptions = CategoryList
        //    };
        //    if (vm.Tag == null)
        //    {
        //        return NotFound();
        //    }
        //    if (currentTagId != vm.Tag.UserProfileId)
        //    {
        //        return NotFound();
        //    }
        //    return View(vm);
        //}

        //// POST: Tag/Edit/5
        //[Authorize]
        //[HttpTag]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, TagCreateViewModel vm)
        //{
        //    try
        //    {
        //        vm.CategoryOptions = _categoryRepository.GetAll();
        //        vm.Tag.Id = id;
        //        vm.Tag.CreateDateTime = DateTime.Now;
        //        _tagRepository.UpdateTag(vm.Tag);

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        vm.CategoryOptions = _categoryRepository.GetAll();
        //        return View(vm);
        //    }
        //}

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


        //        private int GetCurrentUserProfileId()
        //        {
        //            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            return int.Parse(id);
        //        }
    }
  }
