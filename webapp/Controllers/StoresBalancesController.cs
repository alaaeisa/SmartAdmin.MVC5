using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class StoresBalancesController : Controller
    {
        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        string _ControllerName = "Items";
        // GET: StoresBalances
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

                var varData = db.StoresBalances.Where(c=>c.StoreID == _sessionMange.StoreID).Select(x => new SearchVM { ID = x.ID, Name = x.Item.Name, Quantity = x.Quantity }).ToList().AsEnumerable();
                // var varData = db.Items.ToList().AsEnumerable();

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
        #endregion}

        public ActionResult Create()
        {
            InvoiceVM obj = new InvoiceVM();
            return View(obj);
        }

        [HttpPost]
        public JsonResult Create(InvoiceVM MasterObj)
        {
            NotificationMessage ReturnMsg = new NotificationMessage();
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serilizer = new System.Web.Script.Serialization.JavaScriptSerializer();
                InvoiceMaster DBObj = new InvoiceMaster();
                List<InvoiceItem> Details_Item = new List<InvoiceItem>();
                if (!string.IsNullOrEmpty(MasterObj.InvoiceItems))
                {
                    Details_Item = serilizer.Deserialize<List<InvoiceItem>>(MasterObj.InvoiceItems);
                }

               
               
                ReturnMsg = DoNewBalance(DBObj, Details_Item);
                if (!ReturnMsg.Status)
                {
                    msg.Status = false; msg.Message = ReturnMsg.Message; msg.MessageEng = "Sorry  save  Fail";
                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
                }
             
                msg.Status = true; msg.Message = "تم  الحفظ بنجاح"; msg.MessageEng = "Done";

                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
                DbLogs.logData(_ControllerName, "CreateSalesInvoice", ex.Message, "    ");
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        NotificationMessage DoNewBalance(InvoiceMaster DBObj, List<InvoiceItem> DetailsObj)
        {

            NotificationMessage message = new NotificationMessage();
            message.Status = true;
            Common _com = new Common();
            foreach (InvoiceItem item in DetailsObj)
            {

                decimal _BQty = item.Qty;
                if (_com.balancIn(item.ItemID, _sessionMange.StoreID, _BQty, db))
                {
                }
                else
                {
                    message.Status = false; message.Message = "فشل  فى  ترصيد الصنف " + item.ItemID + "كميه سالبه "; msg.MessageEng = "Sorry  save  Fail";
                    return message;
                }
                db.SaveChanges();
            }
            return message;
        }
    }
}