using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using scrolltest.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using DAL;

namespace scrolltest.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


     
        public ActionResult Person(string id)
        {
            if(id == null)
            {
                return RedirectToAction("Index","Home");
            }
            ProductViewModel p = new ProductViewModel();
            var idd = Convert.ToInt32(id);
            
            using (var db = new DirtySecretsDBEntities())
            {
                var image = db.Images.FirstOrDefault(x => x.Id == idd);

                p.Title = image.title;
                p.Image = "/Products/" + image.UserName + "/" + image.fileName;
                p.price = Convert.ToInt32(image.price);
                p.Description = image.description;
                p.Owner = image.UserName;
                p.id = image.UserId;
            }

            return View(p);
        }
 
        [HttpPost]
        public ActionResult Update(int page)
        {
            List<Image> images = new List<Image>();
             
            using (var db = new DirtySecretsDBEntities())
            {
                images = db.Images.ToList();    
            }

            PersonViewModel pv = new PersonViewModel();
            pv.persons = new List<Person>();
            List<Person> templist = new List<Person>();
           
            templist = (from image in images
                        select new Person
                        {
                            Name = "/Products/" + image.UserName + "/" + image.fileName,
                            Grade = image.price.ToString(),
                            UserId = Convert.ToInt32(image.Id),
                            progress = images.Count
                        }).ToList();
           

            for (int i = (page - 1) * 9; i < page * 9; i++)
            {
                if (i < templist.Count)
                {
                    pv.persons.Add(templist[i]);
                }
                else
                {
                    break;
                }
            }

           

            return Json(pv.persons, JsonRequestBehavior.AllowGet);
           
        }


        public ActionResult Welcome()
        {




            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult TandC()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}