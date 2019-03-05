using AMS.Models;
using AMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Controllers
{
    public class UserController : Controller
    {
        dbconn db = new dbconn();
        // GET: User
        public ActionResult Index(AttendanceViewModel vm)
        {
            string email = User.Identity.Name;
            var userdetail = db.Users.Where(u => u.email == email).SingleOrDefault();
            var userid = userdetail.userid;
            DateTime todayDate = Convert.ToDateTime(DateTime.Now);
            var date = todayDate.Date;

            var hasloggedintoday = db.Userlogs.Any(u => u.checkeddate == date && u.userid == userid);
            
            if (hasloggedintoday)
            {
                vm.HasCheckedIn = true;

                var hascheckedouttoday = db.Userlogs.Any(u => u.checkeddate == date & u.userid == userid && u.logouttime != null);

                if(hascheckedouttoday)
                {
                    vm.HasCheckedOut = true;
                }
                else
                {
                    vm.HasCheckedOut = false;
                }
               
            }
            else
            {
                vm.HasCheckedIn = false;
            }
        

            return View(vm);
          
        }

        [HttpPost]
        public ActionResult ButtonAction(string action)
        {
            switch(action)
            {
                case "CheckIn":
                    return CheckIn();
                case "CheckOut":
                    return CheckOut();
                default:
                    return RedirectToAction("Index");
            }
            
        }

        public ActionResult CheckIn()
        {
            AttendanceViewModel vm = new AttendanceViewModel(); 
            string email = User.Identity.Name;

            var userdetail = db.Users.Where(u => u.email == email).SingleOrDefault();
            var checkindate = DateTime.Now;
            var currentday = DateTime.Now;
            var date = currentday.Date;
            var userid = userdetail.userid;
            DateTime todayDate = Convert.ToDateTime(DateTime.Now);

            var hasloggedintoday = db.Userlogs.Any(u => u.checkeddate == date && u.userid == userid);
            if (!hasloggedintoday)
                {
                
                  Userlog ul = new Userlog
                    {
                        checkeddate = date,
                        logintime = checkindate,
                        userid = userdetail.userid
                    };

                    db.Userlogs.Add(ul);
                    db.SaveChanges();
                    
                }

            return RedirectToAction("Index");
        }

        public ActionResult CheckOut()
        {
            AttendanceViewModel vm = new AttendanceViewModel();
            string email = User.Identity.Name;

            var userdetail = db.Users.Where(u => u.email == email).SingleOrDefault();
            var checkindate = DateTime.Now;
            var currentday = DateTime.Now;
            var date = currentday.Date;
            var userid = userdetail.userid;
            DateTime todayDate = Convert.ToDateTime(DateTime.Now);

            var hasloggedintoday = db.Userlogs.Any(u => u.checkeddate == date && u.userid == userid);

            if(hasloggedintoday)
            {
                var LoggedinUser = db.Userlogs.Where(c => c.checkeddate == date && c.userid == userid).Single();
                LoggedinUser.logouttime = DateTime.Now;
                db.Entry(LoggedinUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.Info = "You have Checked out Succesfully ";


            }
            else
            {
                ViewBag.Info = "Please Check in first before you Check Out";
            }

            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult MyAttendance()
        {
            string email = User.Identity.Name;
            var userdetail = db.Users.Where(u => u.email == email).SingleOrDefault();
            var userid = userdetail.userid;
            var attendance = db.Userlogs.Include("User").Where(u => u.userid == userid).ToList();

         

            return View(attendance);
        }

        [HttpPost]
        public ActionResult Myattendance(string start, string end)
        {
            string email = User.Identity.Name;
            var userdetail = db.Users.Where(u => u.email == email).SingleOrDefault();
            var userid = userdetail.userid;
            if (start != "" && end != "")
            {
                DateTime startdate = Convert.ToDateTime(start);
                DateTime enddate = Convert.ToDateTime(end);

                var attendance = db.Userlogs.Where(x => x.checkeddate >= startdate && enddate >= x.checkeddate && x.userid==userid).ToList();
                TempData["Start"] = startdate.ToString("yyyy-MM-dd");
                TempData["End"] = enddate.ToString("yyyy-MM-dd");
                return View(attendance);

            }

            return View();
        }

        public ActionResult Download()
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Username", typeof(string));
            //dt.Columns.Add("Login Time", typeof(string));
            //dt.Columns.Add("Logout Time", typeof(string));
            //dt.Columns.Add("Checked Date", typeof(string));
            //foreach (var a in attendance)
            //{
            //    DataRow dataRow = dt.NewRow();
            //    dataRow["Username"] = a.User.firstname;
            //    dataRow["Login Time"] = a.logintime;
            //    dataRow["Logout Time"] = a.logouttime;
            //    dataRow["Checked Date"] = a.checkeddate;
            //    dt.Rows.Add(dataRow);
            //}
            //GenerateExcelSheet(dt, email);

            return RedirectToAction("MyAttendance");
        }

        public void GenerateExcelSheet(DataTable dt, string s)
        {
            var gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + s + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }

        public ActionResult Viewprofile()
        {

            string email = User.Identity.Name;
            var userdetail = db.Users.Where(u => u.email == email).SingleOrDefault();

            return View(userdetail);
        }



    }
    

    }
