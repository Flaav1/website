using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using scrolltest.ProductViewModels;
using System.Data.SqlClient;
using System.Data;
using DAL;


namespace scrolltest.Controllers
{
    public class MessageController : Controller
    {
        
        // GET: Message
        public ActionResult Write(int id)
        {
         
            if (User.Identity.IsAuthenticated)
            {
                using (var db = new DirtySecretsDBEntities())
                {
                    var usr = db.Users.FirstOrDefault(x => x.Id == id);
                    if(usr == null)
                    {
                        Session["Sent"] = "This seller is not a user anymore!";
                        return View();
                    }
                    else
                    {
                        Session["Receiver"] = usr.UserName;
                        return View();
                    }
                   
                }
                    
               
            }
            else
            {

                return RedirectToAction("Login", "Account");
            }

             

        }

        [HttpPost]
        public ActionResult Write(SendMessageModel mess)
        {
           
            mess.TimeSent = String.Format("{0:g}", DateTime.Now);
            

            using (var db = new DirtySecretsDBEntities())
            {
                db.Messages.Add(new Message()
                {

                    SenderName = User.Identity.Name,
                    SentDate = DateTime.Now,
                    Content = mess.Content,
                    Receiver = (string)Session["Receiver"]

                });

                db.SaveChanges();
            }

            Session["Sent"] = "Your Message has been sent!";
            return View();
        }


        public ActionResult MyMessages()
        {
           
            if(User.Identity.IsAuthenticated)
            {
                var mesages = new List<SendMessageModel>();
                

                using (var db = new DirtySecretsDBEntities())
                {
                   var msgs = db.Messages.Where(x => x.Receiver == User.Identity.Name).ToList();

                    mesages = (from ms in msgs
                               select new SendMessageModel()
                               {
                                  Receiver = ms.Receiver,
                                  TimeSent = String.Format("{0:g}", ms.SentDate),
                                  Content = ms.Content,
                                  SenderName = ms.SenderName,
                                  msgId = ms.Id
                                  
                              }).ToList();
                    var msg = mesages.OrderByDescending(x => x.TimeSent);
                    return View(msg);
                }


            }
            else
            {
                
                return RedirectToAction("Login","Account");
            }
           
        }


        
        public ActionResult Delete(int id)
        {

            using (var db = new DirtySecretsDBEntities())
            {
                var temp = db.Messages.FirstOrDefault(x => x.Id == id);
                db.Messages.Remove(temp);
                db.SaveChanges();
            }

            return RedirectToAction("MyMessages", "Message"); 
        }

    }
}