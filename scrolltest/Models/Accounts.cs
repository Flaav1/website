using System;
using System.ComponentModel.DataAnnotations;
using DAL;
using System.Collections.Generic;
using System.Linq;

namespace scrolltest.Models
{
    public class Accounts
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "A valid Username is required.")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "You have to enter a valid")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "You have to enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "you have to enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }

        public DateTime RegisteredDate { get; set; }

    }

    public class myAccount
    {
        public List<Image> products { get; set; }
        public User user { get; set; }
        public string regdate;

        public myAccount(string name)
        {
            products = new List<Image>();
            user = new User();

            using (var db = new DirtySecretsDBEntities())
            {
                user = db.Users.FirstOrDefault(x => x.UserName == name);
                products = db.Images.Where(x => x.UserId == user.Id).ToList();
                if(user != null)
                {
                    regdate = user.RegistrationDate.ToString("dddd - d/MMMM/yyyy");
                }
               
            }

        }
    }
}