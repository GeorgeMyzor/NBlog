using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ORM;
using ORM.Entities;

namespace MVCNBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new EntityModel())
            {
                var role = new Role {Name = "XDROle"};
                db.Users.Add(new User { Name = "TestUser", Roles = new List<Role>() {role} });
                db.SaveChanges();

                foreach (var user in db.Users)
                {
                    ViewBag.UserName = user.Name;
                    ViewBag.UserRole = user.Roles[0].Name;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}