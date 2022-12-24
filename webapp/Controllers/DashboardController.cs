using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetItemsRates()
        {

            try
            {
                var _items = db.StoresBalances.Where(c => c.StoreID == _sessionMange.StoreID).Select(x => new SearchVM { ID = x.ID, Name = x.Item.Name, Quantity = x.Quantity }).ToList();
                var jsonData = new
                {
                    data = _items
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemsUnbalance()
        {

            try
            {
               
               // var _items = db.Items.Where(z => z.CompanyID == _sessionMange.CompId && z.Is_Unbalance == true).Select(X => X).ToList();
                var _items = db.Items.Select(x => new SearchVM { ID = x.ID, Name = x.Name, Price = x.Price??0 }).ToList();
              
                var jsonData = new
                {
                    data = _items
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }


        

    }
}