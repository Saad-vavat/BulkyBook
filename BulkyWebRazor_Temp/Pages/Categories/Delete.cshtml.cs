using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category? Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? Id)
        {
            if(Id!=null || Id!=0)
            {
                Category = _db.Categories.FirstOrDefault(u => u.Id == Id);
            }
     
        }

        public IActionResult OnPost()
        {
          
                Category? obj = _db.Categories.Find(Category.Id);
                if(obj!=null)
                {
                    _db.Categories.Remove(obj);
                    _db.SaveChanges();
                    TempData["Success"] = "Deleted Successfully!";
                    return RedirectToPage("Index");
                }
             
            return Page();
            
        }
    }
}
