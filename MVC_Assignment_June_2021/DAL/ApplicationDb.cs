using MVC_Assignment_June_2021.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Assignment_June_2021.DAL
{
    public class ApplicationDb : DbContext
    {
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ApplicationDb() : base("MVCAssignmentJune")
        {

        }
    }
}