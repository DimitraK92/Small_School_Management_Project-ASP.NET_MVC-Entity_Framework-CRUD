using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Assignment_June_2021.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(60, ErrorMessage = "First name must be less than 60 characters")]
        [MinLength(2, ErrorMessage = "First name must be more than 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(60, ErrorMessage = "Last name must be less than 60 characters")]
        [MinLength(2, ErrorMessage = "Last name must be more than 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public decimal? Salary { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }
        [Display(Name = "Is Trainer Availabe")]
        public bool ΙsAvailable { get; set; }

        //Navigation Properties
        public virtual ICollection<Category> Categories { get; set; }
        public Trainer()
        {

        }
        public Trainer(string firstName, string lastName, bool isAvailable)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.ΙsAvailable = isAvailable;
        }
        public Trainer(string firstName, string lastName, bool isAvailable, ICollection<Category> categories) : this (firstName, lastName, isAvailable)
        {
            this.Categories = categories;
        }
        public Trainer(string firstName, string lastName, bool isAvailable, decimal salary, DateTime hireDate, ICollection<Category> categories) : this(firstName, lastName, isAvailable, categories)
        {
            this.Salary = salary;
            this.HireDate = hireDate;
        }
    }
}