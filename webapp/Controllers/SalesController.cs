//using CrystalDecisions.CrystalReports.Engine;
//using SmartAdminMvc.Accounts;
//using SmartAdminMvc.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;


//namespace SmartAdminMvc.Controllers
//{
//    public class SalesPrint
//    {
//        public SalesInvoice master { get; set; }
//        public List<Vuw_SalesInvoiceDetails> Details { get; set; }

//    }
//    public class SalesController : Controller
//    {
//        NotificationMessage msg = new NotificationMessage();
//        SessionMange _sessionMange = new SessionMange();
//        TrueDBEntities db = new TrueDBEntities();
//        string _ControllerName = "sales";
//        public PartialViewResult GetBill(int id)
//        {
//            SalesPrint _model = new SalesPrint();
//            ViewBag.AddType = "";
//            ViewBag.CustomerName = "";
//            var _obj = db.SalesInvoices.Where(x => x.InvoiceNo == id && x.CompanyID == _sessionMange.CompId).FirstOrDefault();
//            var _cust = db.Customers.Where(x => x.CustomerId == _obj.CustomerId && x.CompanyID == _sessionMange.CompId).FirstOrDefault();
//            if (_cust != null)
//            {
//                ViewBag.CustomerName = _cust.CustomerName;
//            }

//            var vardata = db.Vuw_SalesInvoiceDetails.Where(x => x.InvoiceNo == id && x.CompanyID == _sessionMange.CompId).ToList();

//            _model.master = _obj;
//            _model.Details = vardata;
//            return PartialView("~/views/Sales/_ShowBill.cshtml", _model);
//        }
//        #region  index 
//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult LoadDataTable()
//        {
//            try
//            {
//                var draw = Request.Form.GetValues("draw").FirstOrDefault();
//                var start = Request.Form.GetValues("start").FirstOrDefault();
//                var length = Request.Form.GetValues("length").FirstOrDefault();

//                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
//                var sortColumnDir = "desc";

//                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
//                int pageSize = length != null ? Convert.ToInt32(length) : 0;
//                int skip = start != null ? Convert.ToInt32(start) : 0;
//                int recordsTotal = 0;
//                var varData = db.SalesInvoices.Where(x => x.CompanyID == _sessionMange.CompId).ToList().AsEnumerable();

//                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
//                {
//                    //  varData = varData.OrderBy(x => x.d_doc_date);
//                }
//                //Search    
//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    varData = varData.Where(m => m.InvoiceDate.Contains(searchValue) || m.InvoiceNo.ToString().Contains(searchValue));
//                    // varData = varData.Where(m=>m.n_document_no==1);
//                }
//                //total number of rows count     
//                recordsTotal = varData.Count();
//                //Paging  
//                var data = varData.Skip(skip).Take(pageSize).ToList();
//                //Returning Json Data    
//                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

//            }
//            catch (Exception)
//            {
//                return Json(data: "Fail", behavior: JsonRequestBehavior.AllowGet);
//            }
//        }
//        #endregion

//        #region  Create  
//        public JsonResult GetData()
//        {
//            var lstItem = db.Items.Select(x => new { x.ItemID, x.ItemName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList();
//            return Json(lstItem, JsonRequestBehavior.AllowGet);
//        }
//        int GetNewInvoiceNo()
//        {
//            int NewtypeId = 1;
//            try
//            {
//                int? ItemID = db.SalesInvoices.Where(x => x.CompanyID == _sessionMange.CompId).Max(p => p.InvoiceNo);
//                NewtypeId = (int)ItemID + 1;
//            }
//            catch (Exception)
//            {
//                NewtypeId = 1;
//            }

//            return NewtypeId;
//        }
//        public ActionResult CreateSalesInvoice()
//        {
//            ViewBag.Invoicetype = new SelectList(db.Options.Where(x => x.OptionId == 1).ToList(), "Code", "OptionName", 1);
//            ViewBag.Tax = db.Configurations.Where(x => x.CompanyID == _sessionMange.CompId).FirstOrDefault().Vat;
//            var directCust = db.Customers.Where(x => x.CompanyID == _sessionMange.CompId && x.IsDirect == true).FirstOrDefault();
//            if (directCust != null)
//            {
//                ViewBag.CustomerId = new SelectList(db.Customers.Select(x => new { x.CustomerId, x.CustomerName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "CustomerId", "CustomerName", directCust.CustomerId);

//            }
//            else
//            {
//                ViewBag.CustomerId = new SelectList(db.Customers.Select(x => new { x.CustomerId, x.CustomerName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "CustomerId", "CustomerName");

//            }
//            SalesInvoice obj = new SalesInvoice();
//            obj.InvoiceNo = GetNewInvoiceNo(); obj.Discount = 0; obj.PaidAmount = 0;
//            obj.InvoiceDate = DateTime.UtcNow.Date.ToString("yyyy/MM/dd");
//            return View(obj);
//        }
//        [HttpPost]
//        public JsonResult CreateSalesInvoice(SalesInvoice MasterObj)
//        {
//            NotificationMessage ReturnMsg = new NotificationMessage();
//            try
//            {
//                System.Web.Script.Serialization.JavaScriptSerializer serilizer = new System.Web.Script.Serialization.JavaScriptSerializer();
//                List<SalesInvoiceDetail> DetailsObj = serilizer.Deserialize<List<SalesInvoiceDetail>>(MasterObj.DetailsListProp);
//                MasterObj.DetailsListProp = "";
//                MasterObj.InvoiceNo = GetNewInvoiceNo();
//                MasterObj.CompanyID = _sessionMange.CompId;

//                ReturnMsg = AccountSalesOperation.DOACC_SalesOperationNew(MasterObj, db);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message;
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }

//                db.SalesInvoices.Add(MasterObj);
//                ReturnMsg = DoNewBalance(MasterObj, DetailsObj);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message; msg.MessageEng = "Sorry  save  Fail";
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }
//                db.SaveChanges();
//                msg.Status = true; msg.Message = "تم  الحفظ بنجاح"; msg.MessageEng = "Done";

//                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
//                DbLogs.logData(_ControllerName, "CreateSalesInvoice", ex.Message, "    ");
//                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//            }
//        }


//        NotificationMessage DoNewBalance(SalesInvoice MasterObj, List<SalesInvoiceDetail> DetailsObj)
//        {

//            NotificationMessage message = new NotificationMessage();
//            message.Status = true;
//            Common _com = new Common();
//            foreach (SalesInvoiceDetail item in DetailsObj)
//            {
//                item.CompanyID = _sessionMange.CompId;
//                item.InvoiceNo = MasterObj.InvoiceNo;
//                var _itemDetails = db.ItemsDetails.Where(x => x.ItemID == item.ItemID && x.nUnitID == item.nUnitID && x.CompanyID == _sessionMange.CompId).FirstOrDefault();
//                decimal _BQty = ((decimal)(_itemDetails.nUnitCoff == null ? 0 : _itemDetails.nUnitCoff)) * (decimal)item.Qty;
//                if (_com.balancOut(item.ItemID, _BQty, _sessionMange.CompId, db))
//                {
//                }
//                else
//                {
//                    message.Status = false; message.Message = "فشل  فى  ترصيد الصنف " + item.ItemID + "كميه سالبه "; msg.MessageEng = "Sorry  save  Fail";
//                    return message;
//                    //Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }
//                db.SalesInvoiceDetails.Add(item);
//            }
//            return message;
//        }
//        #endregion


//        #region  edit


//        public ActionResult EditSales(int id)
//        {
//            ViewBag.Tax = db.Configurations.Where(x => x.CompanyID == _sessionMange.CompId).FirstOrDefault().Vat;
//            SalesInvoice Salesobj = db.SalesInvoices.Where(x => x.InvoiceNo == id && x.CompanyID == _sessionMange.CompId).FirstOrDefault();
//            ViewBag.Invoicetype = new SelectList(db.Options.Where(x => x.OptionId == 1).ToList(), "Code", "OptionName", Salesobj.Invoicetype);
//            ViewBag.CustomerId = new SelectList(db.Customers.Select(x => new { x.CustomerId, x.CustomerName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "CustomerId", "CustomerName", Salesobj.CustomerId);
//            return View(Salesobj);
//        }
//        [HttpPost]
//        public JsonResult EditSales(SalesInvoice MasterObj)
//        {
//            NotificationMessage ReturnMsg = new NotificationMessage();
//            try
//            {
//                System.Web.Script.Serialization.JavaScriptSerializer serilizer = new System.Web.Script.Serialization.JavaScriptSerializer();
//                List<SalesInvoiceDetail> DetailsObj = serilizer.Deserialize<List<SalesInvoiceDetail>>(MasterObj.DetailsListProp);
//                MasterObj.DetailsListProp = "";
//                MasterObj.CompanyID = _sessionMange.CompId;
//                SalesInvoice _Sales = new SalesInvoice();
//                _Sales = db.SalesInvoices.Where(x => x.InvoiceNo == MasterObj.InvoiceNo && x.CompanyID == MasterObj.CompanyID).FirstOrDefault();
//                ReturnMsg = AccountSalesOperation.DOACC_SalesOperationEdit(MasterObj, _Sales, db);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message;
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }
//                #region FillEditpramter
//                _Sales.Notes = MasterObj.Notes;
//                _Sales.CustomerId = MasterObj.CustomerId;
//                _Sales.CustomerName = MasterObj.CustomerName;
//                _Sales.InvoiceDate = MasterObj.InvoiceDate;
//                _Sales.Total = MasterObj.Total;
//                _Sales.net = MasterObj.net;
//                _Sales.TotalDiscount = MasterObj.TotalDiscount;
//                _Sales.TotalTax = MasterObj.TotalTax;
//                _Sales.Discount = MasterObj.Discount;
//                _Sales.Invoicetype = MasterObj.Invoicetype;
//                _Sales.RemainAmount = MasterObj.RemainAmount;
//                _Sales.PaidAmount = MasterObj.PaidAmount;
//                _Sales.IS_DeferredPayment = MasterObj.IS_DeferredPayment;
//                db.Entry(_Sales).State = EntityState.Modified;
//                #endregion


//                var OLdDetailsdata = db.SalesInvoiceDetails.Where(x => x.InvoiceNo == MasterObj.InvoiceNo && x.CompanyID == _sessionMange.CompId).ToList();
//                ReturnMsg = DoOLdItemBalance(MasterObj, OLdDetailsdata);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message;
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }

//                ReturnMsg = DoNewBalance(MasterObj, DetailsObj);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message; msg.MessageEng = "Sorry  save  Fail";
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }
//                //foreach (SalesInvoiceDetail item in DetailsObj)
//                //{
//                //    item.CompanyID = _sessionMange.CompId;
//                //    item.InvoiceNo = MasterObj.InvoiceNo;
//                //    db.SalesInvoiceDetails.Add(item);
//                //}
//                db.SaveChanges();
//                msg.Status = true; msg.Message = "تم  الحفظ بنجاح"; msg.MessageEng = "Done";
//                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
//                DbLogs.logData(_ControllerName, "EditSales", ex.Message, "    ");
//                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//            }
//        }

//        private NotificationMessage DoOLdItemBalance(SalesInvoice masterObj, List<SalesInvoiceDetail> OLdDetailsdata)
//        {
//            NotificationMessage message = new NotificationMessage();
//            message.Status = true;
//            Common _com = new Common();
//            foreach (SalesInvoiceDetail item in OLdDetailsdata)
//            {
//                var _itemDetails = db.ItemsDetails.Where(x => x.ItemID == item.ItemID && x.nUnitID == item.nUnitID && x.CompanyID == _sessionMange.CompId).FirstOrDefault();
//                decimal _BMainQty = ((decimal)(_itemDetails.nUnitCoff == null ? 0 : _itemDetails.nUnitCoff)) * (decimal)item.Qty;
//                if (_com.balancIn(item.ItemID, _BMainQty, _sessionMange.CompId, db))
//                {
//                }
//                else
//                {
//                    message.Status = false;
//                    message.Message = "فشل  فى  ترصيد الصنف " + item.ItemID + "كميه سالبه ";
//                    return message;
//                }
//                db.SalesInvoiceDetails.Remove(item);
//            }
//            return message;
//        }


//        public JsonResult GetSalesInvoiceRow(int InvoiceNo)
//        {
//            try
//            {
//                var vardata = db.Vuw_SalesInvoiceDetails.Where(x => x.InvoiceNo == InvoiceNo && x.CompanyID == _sessionMange.CompId).ToList();
//                var jsonData = new
//                { data = vardata };
//                return Json(jsonData, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                DbLogs.logData(_ControllerName, "GetSalesInvoiceRow", ex.Message, "    ");
//                return Json("Fail", JsonRequestBehavior.AllowGet);
//            }
//        }

//        #endregion

//        #region delete
//        public ActionResult Delete(int ID)
//        {
//            NotificationMessage ReturnMsg = new NotificationMessage();
//            try
//            {
//                var _obj = db.SalesInvoices.Where(X => X.InvoiceNo == ID && X.CompanyID == _sessionMange.CompId).FirstOrDefault();
//                db.SalesInvoices.Remove(_obj);
//                var OLdDetailsdata = db.SalesInvoiceDetails.Where(x => x.InvoiceNo == ID && x.CompanyID == _sessionMange.CompId).ToList();
//                //  DoOLdItemBalance(_obj, OLdDetailsdata);
//                ReturnMsg = AccountSalesOperation.DOACC_SalesOperationDelete(_obj, db);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message;
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }
//                ReturnMsg = DoOLdItemBalance(_obj, OLdDetailsdata);
//                if (!ReturnMsg.Status)
//                {
//                    msg.Status = false; msg.Message = ReturnMsg.Message;
//                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
//                }
//                db.SaveChanges();
//                ReturnMsg.Status = true; ReturnMsg.Message = "done";
//                return Json(data: ReturnMsg, behavior: JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                ReturnMsg.Status = false; ReturnMsg.Message = "done";
//                return Json(data: ReturnMsg, behavior: JsonRequestBehavior.AllowGet);
//            }



//        }
//        #endregion

//        #region print 
//        public ActionResult printSalesBill(int id)
//        {
//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  الطباعة  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "SalesInVoice.rpt"));
//            var lst = db.Vuw_SalesInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId && x.InvoiceNo == id).Select(p => new
//            {

//                SCustomerName = p.SCustomerName ?? "",
//                CustomerId = p.CustomerId,

//                InvoiceNo = p.InvoiceNo,
//                InvoiceDate = p.InvoiceDate ?? "",
//                CustomerName = p.CustomerName ?? "",
//                CustomerPhone = p.CustomerPhone ?? "",
//                CustomerAddress = p.CustomerAddress ?? "",
//                net = p.net ?? 0,
//                Total = p.Total ?? 0,
//                Notes = p.Notes ?? "",
//                CompanyName = p.CompanyName ?? "",
//                CompanyID = p.CompanyID ?? 0,
//                ItemID = p.ItemID ?? 0,
//                Qty = p.Qty ?? 0,
//                ItemTotal = p.ItemTotal ?? 0,
//                Price = p.Price ?? 0,
//                ItemDiscount = p.ItemDiscount ?? 0,
//                ItemTax = p.ItemTax ?? 0,
//                ItemNet = p.ItemNet ?? 0,
//                nUnitID = p.nUnitID ?? 0,

//                sUnitName = p.sUnitName ?? "",
//                ItemName = p.ItemName ?? "",
//            }).ToList();
//            rd.SetDataSource(lst);
//            Response.Buffer = false;
//            Response.ClearContent();
//            Response.ClearHeaders();
//            try
//            {
//                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                stream.Seek(0, SeekOrigin.Begin);
//                return File(stream, "application/pdf", "SalesInVoice.pdf");
//            }
//            catch (Exception ex)
//            {
//                DbLogs.logData(_ControllerName, "print", ex.Message, "    ");
//                throw ex;
//            }
//        }
//        #endregion
//    }
//}