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
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly UserProfileRepository _userProfileRepository;


        public CategoryController(IConfiguration config)
        {
            _categoryRepository = new CategoryRepository(config);
            _userProfileRepository = new UserProfileRepository(config);
        }

        public IActionResult Index()
        {
            var categorys = _categoryRepository.GetAll();
            return View(categorys);
        }

        //public IActionResult UserIndex()
        //{
        //    int UserProfileId = GetCurrentUserProfileId();

        //    List<Category> userCategory = _categoryRepository.GetAllCategoryByUserId(UserProfileId);
        //    return View(userCategory);
        //}

        //public IActionResult Details(int id)
        //{
        //    var category = _categoryRepository.GetPublisedCategoryById(id);
        //    if (category == null)
        //    {
        //        int userId = GetCurrentUserProfileId();
        //        category = _categoryRepository.GetUserCategoryById(id, userId);
        //        if (category == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    return View(category);
        //}

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.Add(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }

        //GET: Categorys/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
           
            return View(category);
        }

        //POST: Category/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.UpdateCategory(category);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }

        // GET: CategoryController/Delete/5
        //[Authorize]
        //public ActionResult Delete(int id)
        //{
        //    int userId = GetCurrentUserProfileId();
        //    Category category = _categoryRepository.GetUserCategoryById(id, userId);

        //    return View(category);
        //}

        //// POST: CategoryController/Delete/5
        //[HttpCategory]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, Category category)
        //{
        //    try
        //    {
        //        _categoryRepository.DeleteCategory(id);

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View(category);
        //    }
        //}


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
