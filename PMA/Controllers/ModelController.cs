using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMA.AppDbContext;
using PMA.Models;
using PMA.Models.ViewModel;

namespace PMA.Controllers
{
    [Authorize(Roles = "Admin, Executive")]
    public class ModelController : Controller
    {
        private readonly PmaDbContext _db;
        [BindProperty]
        public ModelViewModel ModelVM { get; set; }
        public ModelController(PmaDbContext db)
        {
            _db = db;
            ModelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Models.Model()
            };
        }
        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);

            return View(model);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }
        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost()
        {
            if(!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _db.Models.Add(ModelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if(ModelVM.Model==null)
            {
                return NotFound();
            }
            return View(ModelVM);
        }
        [HttpPost,ActionName("Edit")]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _db.Update(ModelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Model model = _db.Models.Find(id);
            if(model == null)
            {
                return NotFound();
            }
            _db.Models.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        [HttpGet("api/models/{MakeID}")]
        public IEnumerable<Model> Models(int MakeID)
        {
            return _db.Models.ToList()
                .Where(m=>m.MakeID==MakeID);
        }

    }
}