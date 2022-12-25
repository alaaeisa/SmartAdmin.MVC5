using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class SettingsController : BaseController
    {
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        // GET: Settings
        string _ControllerName = "Settings";

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditConfig()
        {

            Configuration obj = db.Configurations.FirstOrDefault();
            return View(obj);
        }
        [HttpPost]
        public JsonResult EditConfig(Configuration FormDataObj)
        {
            NotificationMessage msg = new NotificationMessage();
            try
            {
                Configuration _DbObj = new Configuration();
                _DbObj = db.Configurations.FirstOrDefault();
                _DbObj.Vat = FormDataObj.Vat;
                _DbObj.CompanyName = FormDataObj.CompanyName;
                string _FileName = UploadFile();
                _DbObj.Logo = _FileName;

                db.Entry(_DbObj).State = EntityState.Modified;
                db.SaveChanges();
                msg.Status = true;
                msg.Message = "تم  الحفظ بنجاح";
                msg.MessageEng = "Done";
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                DbLogs.logData(_ControllerName, "EditConfig", ex.Message, "    ");
                msg.Status = false;
                msg.Message = "ناسف لم  يتم  الحفظ ";
                msg.MessageEng = "Sorry  save  Fail";
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
        }
    }
}