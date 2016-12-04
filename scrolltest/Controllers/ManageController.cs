using scrolltest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace scrolltest.Controllers
{
    public class ManageController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["Person"].ConnectionString;
        // GET: Manage
        public ActionResult Add()
        {
           
                return View();
        }

        [HttpPost]
        public ActionResult Add(Person p)
        {
            using (var connection = new SqlConnection())
            {
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.ConnectionString = cs;
                cmd.CommandText = "INSERT INTO Person values ('" + p.Name +"','" + p.Grade +"')";
                connection.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Add");
        }

        public ActionResult Delete()
        {

            return View();
        }
    
    }
}