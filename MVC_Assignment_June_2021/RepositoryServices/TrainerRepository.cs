using MVC_Assignment_June_2021.DAL;
using MVC_Assignment_June_2021.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Assignment_June_2021.RepositoryServices
{
    public class TrainerRepository
    {
        ApplicationDb db = new ApplicationDb();

        public List<Trainer> GetAll()
        {
            return db.Trainers.ToList();
        }
        public Trainer GetById(int? id)
        {
            return db.Trainers.Find(id);
        }       
        public void Create (Trainer trainer)
        {
            db.Entry(trainer).State = EntityState.Added;
            db.SaveChanges();
        }
        public void Update(Trainer trainer, IEnumerable <int> SelectedCategoriesIds)
        {
            db.Trainers.Attach(trainer);
            db.Entry(trainer).Collection("Categories").Load();
            trainer.Categories.Clear();
            db.SaveChanges();

            if (!(SelectedCategoriesIds is null))
            {
                foreach (var id in SelectedCategoriesIds)
                {
                    Category category = db.Categories.Find(id);
                    if (category != null)
                    {
                        trainer.Categories.Add(category);
                    }
                }
                db.SaveChanges();
            }

            db.Entry(trainer).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            db.Entry(trainer).State = EntityState.Deleted;
            db.SaveChanges();
        }
        public List<Trainer> GetTrainersPerSelectedCategoryResult(string searchCategory)
        {
            List<Trainer> selectedTrainers = new List<Trainer>();

            var query2 = from trainer in db.Trainers.ToList()
                         group trainer by trainer.Categories into y
                         where y.Key.Any(s => s.Subject.ToUpper().Contains(searchCategory.ToUpper()))
                         select y;

            foreach (var item in query2)
            {
                foreach (var x in item)
                {
                    selectedTrainers.Add(x);
                }
            }

            return selectedTrainers;

        }
        public List<Trainer> FilterTrainer(string searchFirstName, string searchLastName, decimal? searchMinSalary, decimal? searchMaxSalary, DateTime? searchHireDate, bool? searchIsAvailable, string searchCategory)
        {
            var trainers = GetAll();

            if (!String.IsNullOrWhiteSpace(searchFirstName))
            {
                return trainers.Where(x => x.FirstName.ToUpper().Contains(searchFirstName.ToUpper())).ToList();
            }
            if (!String.IsNullOrWhiteSpace(searchLastName))
            {
                return trainers.Where(x => x.LastName.ToUpper().Contains(searchLastName.ToUpper())).ToList();
            }
            if (!(searchMinSalary is null))
            {
                return trainers.Where(x => x.Salary >= searchMinSalary).ToList();
            }
            if (!(searchMaxSalary is null))
            {
                return trainers.Where(x => x.Salary <= searchMaxSalary).ToList();
            }
            if (!(searchHireDate is null))
            {
               return trainers.Where(x => x.HireDate == searchHireDate).ToList();
            }
            if (!(searchIsAvailable is null))
            {
                return trainers.Where(x => x.ΙsAvailable == searchIsAvailable).ToList();
            }
            if (!(searchCategory is null))
            {               
                return GetTrainersPerSelectedCategoryResult(searchCategory).ToList();
            }
            else
            {
                return db.Trainers.ToList();
            }
        }        
        
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}