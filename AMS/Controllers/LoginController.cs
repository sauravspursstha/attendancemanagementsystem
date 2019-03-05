using AMS.Models;
using AMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AMS.Controllers
{
    public class LoginController : Controller
    {
        dbconn db = new dbconn();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                bool uservalid = db.Users.Any(e => e.email == vm.email && e.password == vm.password);


                if (uservalid)
                {
                    var user = db.Users.SingleOrDefault(u => u.email == vm.email);
                    string uid = user.userid.ToString();

                    FormsAuthentication.SetAuthCookie(user.email,false);


                    Session["username"] = user.firstname + " " + user.secondname;
                
                    if(user.usertype=="Admin")
                    {

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    ViewBag.Message = "Username of password is incorrect";
                }
            }
            return View();
         
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}