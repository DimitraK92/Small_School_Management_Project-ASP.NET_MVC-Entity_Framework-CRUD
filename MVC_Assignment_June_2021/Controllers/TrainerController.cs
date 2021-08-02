using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Assignment_June_2021.DAL;
using MVC_Assignment_June_2021.Models;
using MVC_Assignment_June_2021.RepositoryServices;
using PagedList;

namespace MVC_Assignment_June_2021.Controllers
{
    public class TrainerController : Controller
    {
        private ApplicationDb db = new ApplicationDb();
        private TrainerRepository trainerRepository = new TrainerRepository();

        // GET: Trainer
        public ActionResult Index(string searchFirstName, string searchLastName, decimal? searchMinSalary, decimal? searchMaxSalary, DateTime? searchHireDate, bool? searchIsAvailable, string searchCategory, string sortOrder, int? pSize, int? page)
        {

            ViewBag.CurrentFirstName = searchFirstName;
            ViewBag.CurrentLastName = searchLastName;
            ViewBag.CurrentMinSalary = searchMinSalary;
            ViewBag.CurrentMaxSalary = searchMaxSalary;
            ViewBag.CurrentHireDate = searchHireDate;
            ViewBag.CurrentIsAvailable = searchIsAvailable;
            ViewBag.CurrentCategory = searchCategory;
            ViewBag.CurrentSortOrder = sortOrder;


            ViewBag.FNSP = String.IsNullOrEmpty(sortOrder) ? "FirstNameDesc" : "";  //FNSP FirstNameSortParam            
            ViewBag.LNSP = sortOrder == "LastNameAsc" ? "LastNameDesc" : "LastNameAsc";  //LNSP LastNameSortParam
            ViewBag.SSP = sortOrder == "SalaryAsc" ? "SalaryDesc" : "SalaryAsc";  //SSP SalarySortParam
            ViewBag.HDSP = sortOrder == "HireDateAsc" ? "HireDateDesc" : "HireDateAsc";  //HDSP HireDateSortParam
            ViewBag.IASP = sortOrder == "IsAvailableAsc" ? "IsAvailableDesc" : "IsAvailableAsc";  //IASP IsAvailableSortParam      
            ViewBag.CSP = sortOrder == "CategoryAsc" ? "CategoryDesc" : "CategoryAsc";  //CSP CategorySortParam      

            #region Fitering

            var trainers = trainerRepository.FilterTrainer(searchFirstName, searchLastName, searchMinSalary, searchMaxSalary, searchHireDate, searchIsAvailable, searchCategory);

            #endregion

            #region Sorting
            switch (sortOrder)
            {
                case "FirstNameDesc": trainers = trainers.OrderByDescending(x => x.FirstName).ToList(); break;

                case "LastNameAsc": trainers = trainers.OrderBy(x => x.LastName).ToList(); break;
                case "LastNameDesc": trainers = trainers.OrderByDescending(x => x.LastName).ToList(); break;

                case "SalaryAsc": trainers = trainers.OrderBy(x => x.Salary).ToList(); break;
                case "SalaryDesc": trainers = trainers.OrderByDescending(x => x.Salary).ToList(); break;

                case "HireDateAsc": trainers = trainers.OrderBy(x => x.HireDate).ToList(); break;
                case "HireDateDesc": trainers = trainers.OrderByDescending(x => x.HireDate).ToList(); break;

                case "IsAvailableAsc": trainers = trainers.OrderBy(x => x.ΙsAvailable.Equals(true)).ToList(); break;
                case "IsAvailableDesc": trainers = trainers.OrderBy(x => x.ΙsAvailable.Equals(false)).ToList(); break;

                case "CategoryAsc": trainers = trainers.OrderBy(x => x.Categories.Count).ToList(); break;
                case "CategoryDesc": trainers = trainers.OrderByDescending(x => x.Categories.Count).ToList(); break;

                default: trainers = trainers.OrderBy(x => x.FirstName).ToList(); break;
            }
            #endregion

            #region Pagination
           
            int pageSize = pSize ?? 6;
            int pageNumber = page ?? 1;

            #endregion

            return View(trainers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Trainer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = trainerRepository.GetById(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // GET: Trainer/Create
        public ActionResult Create()
        {

            ViewBag.SelectedCategoriesIds = db.Categories.ToList().Select(x => new SelectListItem()
            {
                Value = x.CategoryId.ToString(),
                Text = string.Format($"{x.Subject}")              
            });

            return View();
        }

        // POST: Trainer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainerId,FirstName,LastName,Salary,HireDate,ΙsAvailable")] Trainer trainer, IEnumerable<int> SelectedCategoriesIds)
        {
            if (ModelState.IsValid)
            {
                db.Trainers.Attach(trainer);
                db.Entry(trainer).Collection("Categories").Load();
                trainer.Categories.Clear();
                db.SaveChanges();

                if(!(SelectedCategoriesIds is null))
                {
                    foreach(var id in SelectedCategoriesIds)
                    {
                        Category category = db.Categories.Find(id);
                        if(category !=null)
                        {
                            trainer.Categories.Add(category);
                        }
                    }
                }

                db.Entry(trainer).State = EntityState.Added;
                db.SaveChanges();

                TempData["ShowCreateAlert"] = true;
                TempData["CreateStatus"] = $"You Have Successfully Created this trainer: {trainer.FirstName} {trainer.LastName}";

                return RedirectToAction("Index");
            }

            ViewBag.SelectedCategoriesIds = db.Categories.ToList().Select(x => new SelectListItem()
            {
                Value = x.CategoryId.ToString(),
                Text = string.Format($"{x.Subject}")
            });

            return View(trainer);
        }

        // GET: Trainer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = trainerRepository.GetById(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }

            var categoryIds = trainer.Categories.Select(x => x.CategoryId);

            ViewBag.SelectedCategoriesIds = db.Categories.ToList().Select(x => new SelectListItem()
            {
                Value = x.CategoryId.ToString(),
                Text = string.Format($"{x.Subject}"),
                Selected = categoryIds.Any(a => a == x.CategoryId)
            });

            TempData["ShowEditAlert"] = true;
            TempData["EditStatus"] = $"You Have Successfully Edited this trainer: {trainer.FirstName} {trainer.LastName}";
            return View(trainer);
        }

        // POST: Trainer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainerId,FirstName,LastName,Salary,HireDate,ΙsAvailable")] Trainer trainer, IEnumerable<int> SelectedCategoriesIds)
        {

            if (ModelState.IsValid)
            {
                trainerRepository.Update(trainer, SelectedCategoriesIds);

                return RedirectToAction("Index");
            }

            db.Trainers.Attach(trainer);
            db.Entry(trainer).Collection("Categories").Load();
            var categoryIds = trainer.Categories.Select(x => x.CategoryId);

            ViewBag.SelectedCategoriesIds = db.Categories.ToList().Select(x => new SelectListItem()
            {
                Value = x.CategoryId.ToString(),
                Text = string.Format($"{x.Subject}"),
                Selected = categoryIds.Any(a => a == x.CategoryId)
            });

            TempData["ShowEditAlert"] = true;
            TempData["EditStatus"] = $"You Have Successfully Edited this trainer: {trainer.FirstName} {trainer.LastName}";

            return View(trainer);
        }

        // GET: Trainer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = trainerRepository.GetById(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // POST: Trainer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            trainer.Categories.Clear();           
            db.Entry(trainer).State = EntityState.Deleted;
            db.SaveChanges();

            TempData["ShowDeleteAlert"] = true;
            TempData["DeleteStatus"] = $"You Have Successfully Deleted this trainer: {trainer.FirstName} {trainer.LastName}";
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trainerRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
