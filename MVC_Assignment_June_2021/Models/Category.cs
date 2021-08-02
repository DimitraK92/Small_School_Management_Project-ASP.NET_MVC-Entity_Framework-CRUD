using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Assignment_June_2021.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(), MaxLength(40), MinLength(2)]
        public string Subject { get; set; }
        //Navigation Properties
        public virtual ICollection<Trainer> Trainers { get; set; }
        public Category()
        {

        }
        public Category(string subject)
        {
            this.Subject = subject;
        }
    }
}