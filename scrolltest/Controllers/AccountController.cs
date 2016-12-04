using scrolltest.Models;
using System;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using DAL;
using System.Linq;
using System.Web.Security;


namespace scrolltest.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Accounts ac)
        {

            if(ac.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                ViewBag.Message = "You Have to be at least 18 years old to Create an account";
                return View();
            }
            else
            {

                if (Session["RegError"] != null)
                {
                    ViewBag.Message = "UserName is already in use!";
                    ModelState.SetModelValue("UserName", new ValueProviderResult(null, string.Empty, CultureInfo.InvariantCulture));
                    return View();
                }
                if (Session["RegEmailError"] != null)
                {
                    ViewBag.Message = "Email already registered!";
                    ModelState.SetModelValue("email", new ValueProviderResult(null, string.Empty, CultureInfo.InvariantCulture));
                    return View();
                }
                else
                {
                    var salt = CreateSalt(5);
                    var hashedpwd = getHash(ac.Password, salt);
                    var g = Guid.NewGuid();

                    using (var db = new DirtySecretsDBEntities())
                    {

                        db.Users.Add(new User()
                        {
                            UserName = ac.UserName,
                            salt = salt,
                            Password = hashedpwd,
                            email = ac.email,
                            DateofBirth = ac.DateOfBirth,
                            RegistrationDate = DateTime.Now,
                            VerEmail = false,
                            Seller = false

                        });

                        db.TempMails.Add(new TempMail()
                        {
                            GuidId = g.ToString(),
                            email = ac.email,
                            ExpDate = DateTime.Now.AddHours(4)

                        });

                        db.SaveChanges();


                    }

                    string path = "~/Products/" + ac.UserName;

                    bool folderExists = Directory.Exists(Server.MapPath(path));
                    if (!folderExists)
                        Directory.CreateDirectory(Server.MapPath(path));


                    SendEmail(ac.UserName, ac.email, g);

                    ViewBag.Message = "Your Account Has been registered and you have received an e-mail with the activation link!";
                    return View();
                }
            }


        }

        public ActionResult Confirmation()
        {

            return View();
        }

        public String CreateSalt(int size)
        {

            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string getHash(String input, String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);

            SHA256Managed sha256hashstring = new SHA256Managed();

            byte[] hash = sha256hashstring.ComputeHash(bytes);

            return Convert.ToBase64String(hash);

        }

        public void SendEmail(string username, string useremail, Guid g)
        {

            string useractivation = "http://dirtysecrets.eu/Account/Confirm/" + g.ToString();
            string sbj = "Activation Account";
            string bod = "<h3> Hello " + username + "</h3> <br/>  Click <a href='" + useractivation + "' > Here </a> to confirm your email adress!";

            var mail = new Mailer(useremail, sbj, bod);
            mail.Send();
        }

        public bool VerifiyEmail(string name)
        {
            User usr = new User();
            using (var db = new DirtySecretsDBEntities())
            {
                usr = db.Users.FirstOrDefault(x => x.UserName == name);
            }

            return usr.VerEmail;
                   
        }

        public bool CheckEmail(string mail)
        {
            User usr = new User();
            using (var db =  new DirtySecretsDBEntities())
            {
                usr = db.Users.FirstOrDefault(x => x.UserName == mail);
            }

            if(usr != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void RecoverPwd(string mail, Guid g)
        {

            using (var db = new DirtySecretsDBEntities())
            {
                db.TempMails.Add(new TempMail()
                {
                    email = mail,
                    GuidId = g.ToString(),
                    ExpDate = DateTime.Now.AddMinutes(10)
                             
                });
                db.SaveChanges();
            }
            
            string useractivation = "http://dirtysecrets.eu/Account/ChangePassword/" + g.ToString();
            string Subject = "Password Change Request";
            string Body = "<h3> Hello Mr. " + mail + " </h3> <br/> <p>You have requested a password recovery. <a href=" + useractivation + "> Click Here </a> if you requested a new password or ignore this message (the confirmation link will expire in 24 hours). </p> <br/> <h4> Kind Regards </h4> <h5> DirtySecrets </h5> ";

            var email = new Mailer(mail, Subject, Body);
            email.Send();
            
        }


        [HttpPost]
        public ActionResult CheckName(string name)
        {
            User dbname = new User();

            using (var db = new DirtySecretsDBEntities())
            {
              dbname =  db.Users.FirstOrDefault(x => x.UserName == name);
            }

            if (dbname != null)
            {
                Session["RegError"] = true;
                return Content("Username already in use!");
            }
            else
            {
                Session["RegError"] = null;
                return Content("Username available!");
            }

        }

        [HttpPost]
        public ActionResult CheckMail(string mail)
        {
            User usr = new User();
            using (var db = new DirtySecretsDBEntities())
            {
                usr = db.Users.FirstOrDefault(x => x.email == mail);
            }

            if(usr != null)
            {
                Session["RegEmailError"] = true;
                return Content("An user with this email has already registered!!");
            }
            else
            {
                Session["RegEmailError"] = null;
                return Content("email available!");
            }

        }

        [HttpPost]
        public ActionResult CheckDate(DateTime date)
        {
            if(date > DateTime.Now.AddYears(-18))
            {
                return Content("You have to be at least 18 to Create an account");
            }
            else
            {
                return null;
            }

        }

        public ActionResult Login()
        {
            if(Request.UrlReferrer.LocalPath != null)
            {
                TempData["returnUrl"] = Request.UrlReferrer.LocalPath;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Accounts ac)
        {
            
            User usr = new User();

            if(CheckEmail(ac.UserName))
            {
                if (VerifiyEmail(ac.UserName))
                {
                    using (var db = new DirtySecretsDBEntities())
                    {
                        usr = db.Users.FirstOrDefault(x => x.UserName == ac.UserName);
                    }

                    if (usr.Password == getHash(ac.Password, usr.salt) && usr.UserName == ac.UserName)
                    {
                        FormsAuthentication.SetAuthCookie(ac.UserName, false);
                        
                       

                        if (TempData["returnUrl"] != null)
                        {
                            if((string)TempData["returnUrl"] == "/")
                            {
                                return RedirectToAction("Welcome", "Home");
                            }
                            string tmp = (string)TempData["returnUrl"];
                            string[] url = tmp.Split('/');


                            if(url.Length > 3)
                            {
                                return RedirectToAction(url[2], url[1], new { id = url[3] });
                            }
                            else
                            {
                                return RedirectToAction(url[2], url[1]);
                            }
                            
                         }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Username and Password didn't match.";
                        return View();
                    }

                }
                else
                {
                    ViewBag.Message = "You have a registered account but you haven't validated your e-mail, pleace proceed with the e-mail validation.";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "We haven't found this UserName in our database!";
                return View();

            }




        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Confirm(string email)
        {

            using (var db = new DirtySecretsDBEntities())
            {
                var usr = db.TempMails.FirstOrDefault(x => x.GuidId == email);

                if(usr != null)
                {

                    var ver = db.Users.FirstOrDefault(x => x.email == usr.email);

                        ver.VerEmail = true;
                        db.TempMails.Remove(usr);
                        ViewBag.Message = "Your Account has been activated! You can now Login to your account!";
                        db.SaveChanges();
                        return View();

                     

                }
                else
                {
                    ViewBag.Message = "Your Account is already activated. You can Login to your account.";
                    return View();
                }

            }


        }



        public ActionResult RecoverPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecoverPassword(Accounts ac)
        {
            var g = Guid.NewGuid();
           

            using (var db = new DirtySecretsDBEntities())
            {
                var usr = db.Users.FirstOrDefault(x => x.email == ac.email);

                if(usr != null)
                {
                    RecoverPwd(usr.email, g);
                    ViewBag.Message = "An e-mail has been sent to change your password.";
                    return View();
                }
                else
                {
                    ViewBag.Message = "We didn't find this e-mail in our database.";
                    return View();
                }
            }        
        }

        public ActionResult ChangePassword(string email)
        {

            using (var db = new DirtySecretsDBEntities())
            {
                var tmp = db.TempMails.FirstOrDefault(x => x.GuidId == email);

                if(tmp != null)
                {
                    ViewBag.Message = tmp.email;
                    return View();
                }
                else
                {
                    ViewBag.Empty = "Your link was already used or it expired!";
                    return View();
                }
            }
         
        }

        [HttpPost]
        public ActionResult ChangePassword(Accounts ac)
        {

            using (var db = new DirtySecretsDBEntities())
            {
                var email = db.TempMails.FirstOrDefault(x => x.GuidId == ac.email);

                if(email != null)
                {
                    if (email.ExpDate > DateTime.Now)
                    {


                        var usr = db.Users.FirstOrDefault(x => x.email == email.email);
                        db.TempMails.Remove(email);
                        db.SaveChanges();
                        usr.Password = getHash(ac.Password, usr.salt);
                        ViewBag.Message = "Your Password Has Changed, Please Login with your new Password!";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Your link has expired!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "This link was already used once!";
                    return View();
                }
 

            }

        }



        public ActionResult MyAccount()
        {
           

            if(User.Identity.IsAuthenticated)
            {
               var model = new myAccount(User.Identity.Name); 
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
           
        }
    }
}