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
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.product.GetAll(includeProperties:"Category").ToList();
        
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
        
            ProductVM product = new()
            {
                categoryList =  _unitOfWork.category.GetAll().Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                }),
                product = new Product()
            };
            if(id==null || id==0)
            {
                return View(product);
            }
            else
            {
                //Update
                product.product = _unitOfWork.product.Get(u=>u.ProductId == id);
                return View(product);
            }
      
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVm,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string FileName = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if(!string.IsNullOrEmpty(productVm.product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVm.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);

                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.product.ImageUrl = @"\images\product\" + FileName;
                }
                if(productVm.product.ProductId ==0)
                {
                    _unitOfWork.product.Add(productVm.product);
                }
                else
                {
                    _unitOfWork.product.Update(productVm.product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVm.categoryList = _unitOfWork.category.GetAll().Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                });
            }
            return View(productVm);
        }

  

        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList});
        }

        public IActionResult Delete(int id)
        { 
            var productToBeDeleted = _unitOfWork.product.Get(u=>u.ProductId == id);
            if(productToBeDeleted == null)
            {
                return Json(new {success= false, message="Error While Deleting"});
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.Trim('\\').ToString());
            if(System.IO.File.Exists(oldImagePath))
            {
                    System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully!" });
                
        
        }
        #endregion
    }
}
