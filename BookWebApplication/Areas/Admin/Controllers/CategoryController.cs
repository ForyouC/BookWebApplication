using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookWebApplication.DataAccess.Repository.IRepository;
using BookWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CategoryController(IUnitOfWork UnitOfwork)
        {
            _UnitOfWork = UnitOfwork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if(id == null)
            {
                //this is for create
                return View(category);
            }
            //this is for edit
            category = _UnitOfWork.Category.Get(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    _UnitOfWork.Category.Add(category);
                }
                else
                {
                    _UnitOfWork.Category.Update(category);
                }
                _UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _UnitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _UnitOfWork.Category.Get(id);
            if(objFromDb == null){
                return Json(new { success = false, message = "Error While Deleting" });
            }
            _UnitOfWork.Category.Remove(objFromDb);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}