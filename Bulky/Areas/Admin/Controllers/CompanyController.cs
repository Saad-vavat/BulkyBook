using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      
        public CompanyController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.company.GetAll().ToList();
        
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
     
            if(id==null || id==0)
            {
                return View(new Company());
            }
            else
            {
                //Update
                Company company = _unitOfWork.company.Get(u=>u.Id==id);
                return View(company);
            }
      
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
               
              
                if(CompanyObj.Id==0)
                {
                    _unitOfWork.company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
            return View(CompanyObj);

            }
        }

  

        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.company.GetAll().ToList();
            return Json(new { data = objCompanyList});
        }

        public IActionResult Delete(int id)
        { 
            var companyToBeDeleted = _unitOfWork.product.Get(u=>u.ProductId == id);
            if(companyToBeDeleted == null)
            {
                return Json(new {success= false, message="Error While Deleting"});
            }
         
            _unitOfWork.product.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully!" });
                
        
        }
        #endregion
    }
}
