namespace MVC_Assignment_June_2021.Migrations
{
    using MVC_Assignment_June_2021.DAL;
    using MVC_Assignment_June_2021.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDb context)
        {
            #region Seed Categories

            Category c1 = new Category("Acting");
            Category c2 = new Category("Physical Theatre");
            Category c3 = new Category("Devised Theatre");
            Category c4 = new Category("Anatoly Vasiliev method");
            Category c5 = new Category("Laban Movement Analysis (LMA)");
            Category c6 = new Category("Stanislavski method");
            Category c7 = new Category("Terzopoulos method");
            Category c8 = new Category("Speech Training");
            Category c9 = new Category("Singing");
            Category c10 = new Category("Music");
            Category c11 = new Category("Dance");
            Category c12 = new Category("Modern Dancing");
            Category c13 = new Category("Yoga");
            Category c14 = new Category("Fencing");
            Category c15 = new Category("Dramatology");
            Category c16 = new Category("Literature");
            Category c17 = new Category("Poetry");

            context.Categories.AddOrUpdate(x => x.Subject, c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17);
            context.SaveChanges();
            #endregion

            #region Seed Trainers
            Trainer t1 = new Trainer("Helen", "Potadoul", false, new List<Category> { c15, c16, c17 });
            Trainer t2 = new Trainer("Caroline", "Krad", false, new List<Category> { c13, c14 });
            Trainer t3 = new Trainer("Page", "Rixai", true, 1000m,  new DateTime(2021, 02, 21), new List<Category> { c11, c12 });
            Trainer t4 = new Trainer("Dimi", "Snart", false, 1500m, new DateTime(2020, 10, 30), new List<Category> { c8, c9, c10 });
            Trainer t5 = new Trainer("Margie", "Alepaz", true, 1500m, new DateTime(2020, 11, 30), new List<Category> { c1, c4, c5, c7 });
            Trainer t6 = new Trainer("Mary", "Squat", true, 1000m, new DateTime(2021, 01, 15), new List<Category> { c1, c2, c3, c6 });
            Trainer t7 = new Trainer("Asarti", "Mosh", false, 500.35m, new DateTime(2021, 03, 16), new List<Category> { c5, c11, c12 });
            Trainer t8 = new Trainer("Ter", "Misha", true, 2500.23m, new DateTime(2021, 04, 3), new List<Category> { c1, c6, c13, c17 });
            Trainer t9 = new Trainer("Unis", "Gliarb", true, 4000m, new DateTime(2021, 04, 3), new List<Category> { c1, c2, c7 });
            Trainer t10 = new Trainer("Fiora", "Ford", false, new List<Category> { c4 });
            Trainer t11 = new Trainer("Jerald", "Pior", true, 450.56m, new DateTime(2021, 03, 16), new List<Category> { c9,c10 });
            Trainer t12 = new Trainer("Lisa", "Vevord", false, 5000, new DateTime(2020, 10, 30), new List<Category> { c5, c3, c14 });

            context.Trainers.AddOrUpdate(x => new { x.FirstName, x.LastName }, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            context.SaveChanges();
            #endregion
        }
    }
}
