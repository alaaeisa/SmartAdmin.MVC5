﻿using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SmartAdminMvc.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        string _ControllerName = "Invoice";

        public PartialViewResult GetBill(int id)
        {
            InvoiceVM _model = new InvoiceVM();
            InvoiceMaster Salesobj = db.InvoiceMasters.Where(x => x.ID == id).FirstOrDefault();
            _model = new InvoiceVM().Convert(Salesobj);
            return PartialView("~/views/Invoice/_ShowInvoice.cshtml", _model);
        }

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
                var varData = db.InvoiceMasters.ToList().AsEnumerable();
                //  var varData = db.InvoiceMasters.Select(x => new InvoiceVM().Convert(x,true)).ToList().AsEnumerable(); 
                varData = varData.OrderByDescending(x => x.ID);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //  varData = varData.OrderBy(x => x.d_doc_date);
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    varData = varData.Where(m => m.Date.ToString().Contains(searchValue) || m.ID.ToString().Contains(searchValue));
                    // varData = varData.Where(m=>m.n_document_no==1);
                }
                //total number of rows count     
                recordsTotal = varData.Count();
                //Paging  
                var data = varData.Skip(skip).Take(pageSize).ToList();
                var data1 = data.Select(x => new InvoiceVM().Convert(x, true)).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data1 });

            }
            catch (Exception ex)
            {
                return Json(data: "Fail", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateExcel()
        {

            var table = new System.Data.DataTable("ExceFile");

            var data = db.InvoiceMasters.ToList();
            var varData = data.Select(x => new InvoiceVM().Convert(x, true)).ToList();
            table.Columns.Add("الكود", typeof(int));
            table.Columns.Add("التاريخ", typeof(string));
            table.Columns.Add("العميل", typeof(string));
            table.Columns.Add("رقم اللوحة", typeof(string));
            table.Columns.Add("الموبايل", typeof(string));
            table.Columns.Add("الماركة", typeof(string));
            table.Columns.Add("الموديل", typeof(string));
            table.Columns.Add("الصافى", typeof(decimal));
            table.Columns.Add("ملاحظات", typeof(string));

        

            foreach (var item in varData)
            {
                table.Rows.Add(item.ID, item.DateStr, item.CustomerName, item.PlateNumber, item.CustomerPhone, item.BrandName, item.ModelName, item.NetAmount, item.Notes);
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

        #region Add
        int GetNewInvoiceNo()
        {
            int NewtypeId = 1;
            try
            {
                int? ItemID = db.InvoiceMasters.Max(p => p.ID);
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
            InvoiceVM obj = new InvoiceVM();
            ViewBag.CustomerId = new SelectList(db.Customers.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name");
            ViewBag.BrandId = new SelectList(db.CarBrands.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name");
            ViewBag.Tax = db.Configurations.FirstOrDefault().Vat;
            ViewBag.ModelId = 0;
            obj.ID = GetNewInvoiceNo();
            obj.DateStr = DateTime.Now.ToString("yyyy/MM/dd");
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

                List<InvoiceService> Details_Service = new List<InvoiceService>();
                if (!string.IsNullOrEmpty(MasterObj.InvoiceServices))
                {
                    Details_Service = serilizer.Deserialize<List<InvoiceService>>(MasterObj.InvoiceServices);
                }

                DBObj.ID = GetNewInvoiceNo();

                DBObj.Date = MasterObj.Date;

                DBObj.StoreID = _sessionMange.StoreID;
                DBObj.CustomerId = MasterObj.CustomerId;
                DBObj.Notes = MasterObj.Notes;
     
                DBObj.ChassisNo = MasterObj.ChassisNo;
                DBObj.PlateNumber = MasterObj.PlateNumber;
                DBObj.BrandId = MasterObj.BrandId;
                DBObj.ModelId = MasterObj.ModelId;
                DBObj.km = MasterObj.km;
                DBObj.Total = MasterObj.Total;
                DBObj.NetAmount = MasterObj.NetAmount;
                DBObj.NetAmountWithOutTax = MasterObj.NetAmountWithOutTax;
                DBObj.InvoiceDiscount = MasterObj.InvoiceDiscount;
                DBObj.TotalDiscount = MasterObj.TotalDiscount;
                DBObj.InvoiceTax = MasterObj.InvoiceTax;
                DBObj.PaidAmount = MasterObj.PaidAmount;
                DBObj.RemainingAmount = MasterObj.RemainingAmount;

                db.InvoiceMasters.Add(DBObj);
                ReturnMsg = DoNewBalance(DBObj, Details_Item);
                if (!ReturnMsg.Status)
                {
                    msg.Status = false; msg.Message = ReturnMsg.Message; msg.MessageEng = "Sorry  save  Fail";
                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
                }
               var _NewServiceId =  GetNewServiceId();
                int Count = 0;
                foreach (InvoiceService Service in Details_Service)
                {
                    Service.InvoiceID = DBObj.ID;
                    Service.ID = _NewServiceId + Count;
                    db.InvoiceServices.Add(Service);
                    Count++;
                }
                db.SaveChanges();
                msg.Status = true; msg.Message = " : تم  الحفظ بنجاح الفاتورة  رقم  " + DBObj.ID + " "; msg.MessageEng = "Done";

                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
                DbLogs.logData(_ControllerName, "CreateSalesInvoice", ex.InnerException.InnerException.Message, "    ");
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

                item.InvoiceID = DBObj.ID;
                decimal _BQty = item.Qty;
                if (_com.balancOut(item.ItemID, _sessionMange.StoreID, _BQty, db))
                {
                }
                else
                {
                    message.Status = false; message.Message = "فشل  فى  ترصيد الصنف " + item.ItemID + "كميه سالبه "; msg.MessageEng = "Sorry  save  Fail";
                    return message;
                }
                db.InvoiceItems.Add(item);
            }
            return message;
        }

        #endregion


        #region Edit

        public ActionResult Edit(int id)
        {
            InvoiceMaster Salesobj = db.InvoiceMasters.Where(x => x.ID == id).FirstOrDefault();
            InvoiceVM Obj = new InvoiceVM().Convert(Salesobj);
            ViewBag.CustomerId = new SelectList(db.Customers.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", Obj.CustomerId);
            ViewBag.BrandId = new SelectList(db.CarBrands.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", Obj.BrandId);
            ViewBag.Tax = db.Configurations.FirstOrDefault().Vat;
            ViewBag.ModelId = Obj.ModelId;
            return View(Obj);
        }
        [HttpPost]
        public JsonResult Edit(InvoiceVM MasterObj)
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

                List<InvoiceService> Details_Service = new List<InvoiceService>();
                if (!string.IsNullOrEmpty(MasterObj.InvoiceServices))
                {
                    Details_Service = serilizer.Deserialize<List<InvoiceService>>(MasterObj.InvoiceServices);
                }
                #region FillEditpramter

                DBObj = db.InvoiceMasters.Where(x => x.ID == MasterObj.ID).FirstOrDefault();
                var OLdDetailsdata = db.InvoiceItems.Where(x => x.InvoiceID == MasterObj.ID).ToList();
                ReturnMsg = DoOLdItemBalance(DBObj, OLdDetailsdata);
                if (!ReturnMsg.Status)
                {
                    msg.Status = false; msg.Message = ReturnMsg.Message;
                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
                }

                DBObj.Date = MasterObj.Date;
                DBObj.StoreID = _sessionMange.StoreID;
                DBObj.CustomerId = MasterObj.CustomerId;
                DBObj.Notes = MasterObj.Notes;
   
                DBObj.ChassisNo = MasterObj.ChassisNo;
                DBObj.PlateNumber = MasterObj.PlateNumber;
                DBObj.BrandId = MasterObj.BrandId;
                DBObj.ModelId = MasterObj.ModelId;
                DBObj.km = MasterObj.km;
                DBObj.Total = MasterObj.Total;
                DBObj.NetAmount = MasterObj.NetAmount;
                DBObj.NetAmountWithOutTax = MasterObj.NetAmountWithOutTax;
                DBObj.InvoiceDiscount = MasterObj.InvoiceDiscount;
                DBObj.TotalDiscount = MasterObj.TotalDiscount;
                DBObj.InvoiceTax = MasterObj.InvoiceTax;
                DBObj.PaidAmount = MasterObj.PaidAmount;
                DBObj.RemainingAmount = MasterObj.RemainingAmount;
                db.Entry(DBObj).State = EntityState.Modified;
                #endregion


              

                ReturnMsg = DoNewBalance(DBObj, Details_Item);
                if (!ReturnMsg.Status)
                {
                    msg.Status = false; msg.Message = ReturnMsg.Message; msg.MessageEng = "Sorry  save  Fail";
                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
                }

                db.InvoiceServices.RemoveRange(DBObj.InvoiceServices);
                var _NewServiceId = GetNewServiceId();
                int Count = 0;
                foreach (InvoiceService Service in Details_Service)
                {
                    Service.InvoiceID = DBObj.ID;
                    Service.ID = _NewServiceId + Count;
                    db.InvoiceServices.Add(Service);
                    Count++;
                }
                db.SaveChanges();
                msg.Status = true; msg.Message = " : تم  الحفظ بنجاح الفاتورة  رقم  "+  DBObj.ID +  " "; msg.MessageEng = "Done";
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.Status = false; msg.Message = "ناسف لم  يتم  الحفظ "; msg.MessageEng = "Sorry  save  Fail";
                DbLogs.logData(_ControllerName, "Edit", ex.Message, "    ");
                return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
            }
        }

        private int GetNewServiceId()
        {
            int NewtypeId = 1;
            try
            {
                int? ItemID = db.InvoiceServices.Max(p => p.ID);
                NewtypeId = (int)ItemID + 1;
            }
            catch (Exception)
            {
                NewtypeId = 1;
            }

            return NewtypeId;
        }

        private NotificationMessage DoOLdItemBalance(InvoiceMaster MasterObj, List<InvoiceItem> OLdDetailsdata)
        {
            NotificationMessage message = new NotificationMessage();
            message.Status = true;
            Common _com = new Common();
            foreach (InvoiceItem item in OLdDetailsdata)
            {

                decimal _BMainQty = item.Qty;
                if (_com.balancIn(item.ItemID, MasterObj.StoreID, _BMainQty, db))
                {
                }
                else
                {
                    message.Status = false;
                    message.Message = "فشل  فى  ترصيد الصنف " + item.ItemID + "كميه سالبه ";
                    return message;
                }
                db.InvoiceItems.Remove(item);
            }
            return message;
        }


        #endregion


        #region delete
        public ActionResult Delete(int ID)
        {
            NotificationMessage ReturnMsg = new NotificationMessage();
            try
            {
                var _obj = db.InvoiceMasters.Where(X => X.ID == ID).FirstOrDefault();
                db.InvoiceServices.RemoveRange(_obj.InvoiceServices);
                db.InvoiceMasters.Remove(_obj);
                var OLdDetailsdata = db.InvoiceItems.Where(x => x.InvoiceID == ID).ToList();
                ReturnMsg = DoOLdItemBalance(_obj, OLdDetailsdata);
                if (!ReturnMsg.Status)
                {
                    msg.Status = false; msg.Message = ReturnMsg.Message;
                    return Json(data: msg, behavior: JsonRequestBehavior.AllowGet);
                }
                db.SaveChanges();
                ReturnMsg.Status = true; ReturnMsg.Message = "done";
                return Json(data: ReturnMsg, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                ReturnMsg.Status = false; ReturnMsg.Message = "done";
                return Json(data: ReturnMsg, behavior: JsonRequestBehavior.AllowGet);
            }


        }
        #endregion
    }
}