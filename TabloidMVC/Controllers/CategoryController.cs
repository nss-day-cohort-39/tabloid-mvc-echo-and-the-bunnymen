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

        // GET: Categorys/Edit/5
        //[Authorize]
        //public ActionResult Edit(int id)
        //{
        //    int currentCategoryId = GetCurrentUserProfileId();
        //    Category category = _categoryRepository.GetUserCategoryById(id, currentCategoryId);
        //    List<Category> CategoryList = _categoryRepository.GetAll();
        //    CategoryCreateViewModel vm = new CategoryCreateViewModel
        //    {
        //        Category = category,
        //        CategoryOptions = CategoryList
        //    };
        //    if (vm.Category == null)
        //    {
        //        return NotFound();
        //    }
        //    if (currentCategoryId != vm.Category.UserProfileId)
        //    {
        //        return NotFound();
        //    }
        //    return View(vm);
        //}

        // POST: Category/Edit/5
        //[Authorize]
        //[HttpCategory]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, CategoryCreateViewModel vm)
        //{
        //    try
        //    {
        //        vm.CategoryOptions = _categoryRepository.GetAll();
        //        vm.Category.Id = id;
        //        vm.Category.CreateDateTime = DateTime.Now;
        //        _categoryRepository.UpdateCategory(vm.Category);

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        vm.CategoryOptions = _categoryRepository.GetAll();
        //        return View(vm);
        //    }
        //}

        // GET: CategoryController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            
            Category category = _categoryRepository.GetCategoryById(id);

            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.DeleteCategory(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
