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
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;

namespace PMA.Controllers
{
    [Authorize(Roles = "Admin, Executive")]
    public class SubjectController : Controller
    {
        private readonly PmaDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;
        

        [BindProperty]
        public SubjectViewModel SubjectVM { get; set; }
        public SubjectController(PmaDbContext db, HostingEnvironment hostingEnvironment )

        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            SubjectVM = new SubjectViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Subject = new Models.Subject()
            };
        }
        public IActionResult Index()
        {
            var Subjects = _db.Subjects.Include(m => m.Make).Include(m => m.Model);

            return View(Subjects.ToList());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SubjectVM.Subject = _db.Subjects.SingleOrDefault(s => s.Id == id);
            SubjectVM.Models = _db.Models.Where(m => m.MakeID == SubjectVM.Subject.MakeID);
            if (SubjectVM.Subject == null)
            {
                return NotFound();
            }
            return View(SubjectVM);
                 
        }
        public IActionResult Create()
        {
            return View(SubjectVM);
        }
        [HttpPost, ActionName("Create")]
       
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                SubjectVM.Makes = _db.Makes.ToList();
                SubjectVM.Models = _db.Models.ToList();
                return View(SubjectVM);
            }
            _db.Subjects.Add(SubjectVM.Subject);
            _db.SaveChanges();

            var SubjectID = SubjectVM.Subject.Id;
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var SavedSubject = _db.Subjects.Find(SubjectID);
            if(files.Count!=0)
            {
                var ImagePath = @"images\subject\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + SubjectID + Extension;
                var AbsImagePath = Path.Combine(wwwrootPath, RelativeImagePath);

                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                SavedSubject.ImagePath = RelativeImagePath;
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        //        public IActionResult Edit(int id)
        //        {
        //            SubjectVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
        //            if (ModelVM.Model == null)
        //            {
        //                return NotFound();
        //            }
        //            return View(ModelVM);
        //        }
        //        [HttpPost, ActionName("Edit")]
        //        public IActionResult EditPost()
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(ModelVM);
        //            }
        //            _db.Update(ModelVM.Model);
        //            _db.SaveChanges();
        //            return RedirectToAction(nameof(Index));
        //        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Subject subject = _db.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            _db.Subjects.Remove(subject);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}