using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace scrolltest.Models
{
    public class ProductViewModel
    {
        public int price { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public int id { get; set; }
           
    }

    public class Person
    {
        
        public int UserId { get; set; }
        public string  Name { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public int progress { get; set; }
    }

    public class PersonContext : DbContext
    {
        public DbSet<Person> persondb { get; set; }
    }
    public class PersonViewModel
    {
        public List<Person> persons { get; set; }
    }
}