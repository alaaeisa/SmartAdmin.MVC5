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
        public ActionResult ItemIndex()
        {
            return View();
        }
        public ActionResult ItemLoadDataTable()
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

                var varData = db.BalanceMasters.Select(x => new SearchVM { ID = x.ID, Name = x.Store.Name, Date = x.Date.ToString() }).ToList().AsEnumerable();
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
            ViewBag.StoreID = new SelectList(db.Stores.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", _sessionMange.StoreID);
            obj.ID = GetNewNo();
            obj.DateStr = DateTime.Now.ToString("yyyy/MM/dd");
            return View(obj);
        }

        private int GetNewNo()
        {
            int NewtypeId = 1;
            try
            {
                int? ItemID = db.BalanceMasters.Max(p => p.ID);
                NewtypeId = (int)ItemID + 1;
            }
            catch (Exception)
            {
                NewtypeId = 1;
            }

            return NewtypeId;
        }

        [HttpPost]
        public JsonResult Create(InvoiceVM MasterObj)
        {
            NotificationMessage ReturnMsg = new NotificationMessage();
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serilizer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BalanceMaster DBObj = new BalanceMaster();
                List<InvoiceItem> Details_Item = new List<InvoiceItem>();
                if (!string.IsNullOrEmpty(MasterObj.InvoiceItems))
                {
                    Details_Item = serilizer.Deserialize<List<InvoiceItem>>(MasterObj.InvoiceItems);
                }

                DBObj.ID = GetNewNo();

                DBObj.Date = MasterObj.Date;
                DBObj.StoreId = MasterObj.StoreId;
                DBObj.Notes = MasterObj.Notes;

                db.BalanceMasters.Add(DBObj);

                ReturnMsg = DoNewBalance(DBObj, Details_Item);
                if (!ReturnMsg.Status)
                {
                    msg.Status = false; msg.Message = ReturnMsg.Message; msg.MessageEng = "Sorry  save  Fail";
                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
                }
                db.SaveChanges();
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

        NotificationMessage DoNewBalance(BalanceMaster DBObj, List<InvoiceItem> DetailsObj)
        {

            NotificationMessage message = new NotificationMessage();
            message.Status = true;
            Common _com = new Common();
            foreach (InvoiceItem item in DetailsObj)
            {

                BalanceItem balanceItem = new BalanceItem();
                balanceItem.BalanceID = DBObj.ID;
                balanceItem.ItemID = item.ItemID;
                balanceItem.SerialNo = item.SerialNo;
                balanceItem.Qty = item.Qty;
                decimal _BQty = item.Qty;
                if (_com.balancIn(item.ItemID, DBObj.StoreId, _BQty, db))
                {
                }
                else
                {
                    message.Status = false; message.Message = "فشل  فى  ترصيد الصنف " + item.ItemID + "كميه سالبه "; msg.MessageEng = "Sorry  save  Fail";
                    return message;
                }
                db.BalanceItems.Add(balanceItem);
            
            }
            return message;
        }



        public ActionResult Edit(int id)
        {
            BalanceMaster obj = db.BalanceMasters.Where(x => x.ID == id).FirstOrDefault();
            InvoiceVM Obj = new InvoiceVM().ConvertBalance(obj);
            ViewBag.StoreID = new SelectList(db.Stores.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", obj.StoreId);
       
            return View(Obj);
        }
    }
}