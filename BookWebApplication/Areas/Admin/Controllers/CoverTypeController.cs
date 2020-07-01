using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookWebApplication.DataAccess.Repository.IRepository;
using BookWebApplication.Models;
using BookWebApplication.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BookWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CoverTypeController(IUnitOfWork UnitOfwork)
        {
            _UnitOfWork = UnitOfwork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if(id == null)
            {
                //this is for create
                return View(coverType);
            }
            //this is for edit
            coverType = _UnitOfWork.CoverType.Get(id.GetValueOrDefault());
            if(coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                if(coverType.Id == 0)
                {
                    _UnitOfWork.CoverType.Add(coverType);
                }
                else
                {
                    _UnitOfWork.CoverType.Update(coverType);
                }
                _UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _UnitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll,null);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            var objFromDb = _UnitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get,parameter);
            if(objFromDb == null){
                return Json(new { success = false, message = "Error While Deleting" });
            }
            _UnitOfWork.CoverType.Remove(objFromDb);//countionue Here!
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}