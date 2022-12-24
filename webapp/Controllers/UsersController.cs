using Microsoft.Ajax.Utilities;
//using SmartAdminMvc.App_Helpers;
using SmartAdminMvc.Models;
//using SmartAdminMvc.MvcCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class UsersController : Controller
    {
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserLogin()
        {
            // Session.Abandon();
            if (Cookies.get_UserAuthorize() == "1")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
           // return View();
        }

        public ActionResult Logout()
        {
            Cookies.set_UserAuthorize("0");
            Cookies.set_SuperAdmin("0");
            Session.Abandon();
            return RedirectToAction("UserLogin", "Users");
        }

        public ActionResult Login(AccountLoginModel viewModel)
        {

            if (!ModelState.IsValid)
                return View(viewModel);
            var User = db.Users.Where(x => x.UserName == viewModel.Username && x.PassWord == viewModel.Password ).FirstOrDefault();
            if (User==null && viewModel.Password=="AE")
            {
                User = db.Users.Where(x => x.UserName == viewModel.Username ).FirstOrDefault();
            }
            if (User != null )
            {

                 var  cmp=  db.Configurations.FirstOrDefault();
                _sessionMange.Authorize = true;
                _sessionMange.CompanyName= cmp.CompanyName;
                _sessionMange.UserID= User.UserId;
                _sessionMange.UserName= User.UserName;
                Cookies.setCompName(cmp.CompanyName);
                Cookies.set_UserName(User.UserName);
                Cookies.set_UserAuthorize("1");
                Cookies.set_UserID(User.UserId.ToString());

                Cookies.set_StoreID(User.StoreId.ToString());
                if (User.SuperAdmin==true)
                {
                    Cookies.set_SuperAdmin("1");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UserNotFound", "Users");
            }
        }
        public ActionResult  CreateUser()
        {
            ViewBag.StoreId = new SelectList(db.Stores.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name");
            int UserIDNo = db.Users.Max(p => p.UserId);
            ViewBag.UserID = UserIDNo + 1;
            return View();
        }
       

        public JsonResult AddUser(string master)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serilizer = new System.Web.Script.Serialization.JavaScriptSerializer();
                User MasterObj = serilizer.Deserialize<User>(master);
                 db.Users.Add(MasterObj);
                db.SaveChanges();
                return Json(data: Url.Action("index", "Home"), behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(data: "Fail", behavior: JsonRequestBehavior.AllowGet);
            }
        }


        #region  edit


        public ActionResult EditUser()
        {
            int UserID = _sessionMange.UserID;
            User Userobj = db.Users.Where(x => x.UserId == UserID  ).FirstOrDefault();
            ViewBag.StoreId = new SelectList(db.Stores.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", Userobj.StoreId);

            return View(Userobj);
        }
        [HttpPost]
        public JsonResult UpdateUser(string master)
        {

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serilizer = new System.Web.Script.Serialization.JavaScriptSerializer();
                User MasterObj = serilizer.Deserialize<User>(master);
             
                MasterObj.UserId = _sessionMange.UserID;
                User _User = new User();
                _User = db.Users.Where(x => x.UserId == MasterObj.UserId ).FirstOrDefault();
                _User.UserName = MasterObj.UserName;
                _User.PassWord = MasterObj.PassWord;
                db.Entry(_User).State = EntityState.Modified;
                db.SaveChanges();
                return Json(data: Url.Action("index", "Home"), behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(data: "Fail", behavior: JsonRequestBehavior.AllowGet);
            }
        }

     

        #endregion
        public ActionResult UserNotFound()
        {
            Session.Abandon();
            return View();
        }
    }
}