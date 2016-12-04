using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using scrolltest.Models;
using System.IO;
using DAL;
using System.Web.Security;
using System.Security.Authentication;

namespace scrolltest.Controllers
{
    public class ProductController : Controller
    {

        
        public ActionResult Addfile()
        {
            if(User.Identity.IsAuthenticated)
            { 
                return View();
            }
            else
            {
               return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Addfile(PostedProduct prd)
        {
            string fileName;
            string path;
            int fileSize;

                foreach (var file in prd.files)
                {
                    if (file.ContentLength > 0)
                    {
                         
                         fileName = Path.GetFileName(file.FileName);

                    if(!file.ContentType.Contains("image"))
                        {
                          ViewBag.Message = "You can only upload image files of type (.jpeg, .png or .gif)";
                          return View();       
                        }


                         path = Path.Combine(Server.MapPath("~/Products/" + User.Identity.Name) , fileName);
                        fileSize = file.ContentLength;
                    if(System.IO.File.Exists(path))
                    {
                        ViewBag.Message = "A file with that name already exists in our database.";
                        return View();
                       
                    }
                    else
                    {
                        
                        file.SaveAs(path);

                        using (var db = new DirtySecretsDBEntities())
                        {
                            var id = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                            db.Images.Add(new Image()
                            {
                                title = prd.title,
                                description = prd.description,
                                price = prd.price,
                                UserId = id.Id,
                                UserName = User.Identity.Name,
                                fileName = fileName,
                                path = path,
                                fileSize = fileSize
                           
                            });

                            db.SaveChanges();
                        }
                    }
                    }
                }

            ViewBag.Message = "Your Photo has been added!";
            return View();
        }
    }
}