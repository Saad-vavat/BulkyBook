﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.product.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Details(int Id)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.product.Get(u => u.ProductId == Id, includeProperties: "Category"),
                Count = 1,
                ProductId = Id
            };
           
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.ApplicationUserId = userId;
            cart.Product = null;
            ShoppingCart cartFromDb = _unitOfWork.shoppingCart.Get(u=>u.ApplicationUserId == userId && u.ProductId== cart.ProductId);
            if(cartFromDb != null)
            {
                cartFromDb.Count += cart.Count;
                _unitOfWork.shoppingCart.Update(cartFromDb);
            }
            else
            {
                _unitOfWork.shoppingCart.Add(cart);
            }
            TempData["success"] = "Cart Updated Successfully";
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}