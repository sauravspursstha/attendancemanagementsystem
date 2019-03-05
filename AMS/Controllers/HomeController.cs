using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AMS.Models;
using System.Web.Mvc;
using AMS.Models.ViewModel;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace AMS.Controllers
{
    public class HomeController : Controller
    {
        dbconn db = new dbconn();

        [AllowAnonymous]
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ViewUsers()
        {
            var getdata = db.Userlogs.FirstOrDefault(x => x.userid == 1);
            //var username = getdata.User.secondname;
            //var user = new User();
            //var userlog = new Userlog();

            //   if(Session["userid"] != null)

            string user = User.Identity.Name;
            var userlist = db.Users.ToList();
            List<DisplayUserViewModel> vm = new List<DisplayUserViewModel>();
            foreach (var d in userlist)
            {

                vm.Add(new DisplayUserViewModel
                {
                    userid = d.userid,
                    firstname = d.firstname,
                    secondname = d.secondname,
                    address = d.address,
                    email = d.email,
                    phone = d.phone,
                    usertype = d.usertype,

                });
            }

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Register(RegisterUserViewModel viewModel)
        {
            var checkuser = db.Users.Where(c => c.email == viewModel.email).ToList();

            if (checkuser.Count <= 1)

            {
                if (ModelState.IsValid)
                {

                    db.Users.Add(new User
                    {
                        firstname = viewModel.firstname,
                        secondname = viewModel.secondname,
                        address = viewModel.address,
                        email = viewModel.email,
                        password = viewModel.confirmpassword,
                        phone = viewModel.phone,
                        usertype = viewModel.usertype.ToString()
                    });
                    db.SaveChanges();
                    SendEmail(viewModel.email, viewModel.password);

                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public void SendEmail(string emailID, string password)
        {
            var fromEmail = new MailAddress("documentmanagement07@gmail.com", "DMS");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "document147@";
            string subject = "Your account is successfully created!";

            string body = "Congratulation!! Your account is successfully created. You can access your account with given credentials. Thank You" +
                " Email : " + emailID + "</br> Password : " + password + ".";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Attendance(int? id = 0)
        {
            if (id == 0)
            {
                var attendance = db.Userlogs.Include("User").ToList();
             

                return View(attendance);
            }
            else
            {
                var attendance = db.Userlogs.Include("User").Where(u => u.userid == id).ToList();

                return View(attendance);
            }
            
        }


        [HttpPost]
        [Authorize(Roles ="Admin")]
       public ActionResult Attendance(string start, string end, int? id)
        {
           
            
                if (start != "" && end != "")
                {
                if (id == null)
                {
                    DateTime startdate = Convert.ToDateTime(start);
                    DateTime enddate = Convert.ToDateTime(end);

                    var attendance = db.Userlogs.Where(x => x.checkeddate >= startdate && enddate >= x.checkeddate).ToList();
                    TempData["Start"] = startdate.ToString("yyyy-MM-dd");
                    TempData["End"] = enddate.ToString("yyyy-MM-dd");


                    return View(attendance);

                }
                else
                {
                    DateTime startdate = Convert.ToDateTime(start);
                    DateTime enddate = Convert.ToDateTime(end);

                    var attendance = db.Userlogs.Where(x => x.checkeddate >= startdate && enddate >= x.checkeddate && x.userid ==id).ToList();
                    TempData["Start"] = startdate.ToString("yyyy-MM-dd");
                    TempData["End"] = enddate.ToString("yyyy-MM-dd");


                    return View(attendance);

                }
            }
        
            else
            {

                return RedirectToAction("Attendance");

            }
       
        }


    



    }
}
