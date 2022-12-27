﻿using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.WebPages;

namespace SmartAdminMvc.Controllers
{
    public class ItemsController : Controller
    {
        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        string _ControllerName = "Items";
      
        #region logicMethoud
        public JsonResult GetData()
        {

            var lstItem = db.CarModels.ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetItemData(int ModelId)
        {

            var lst = new List<DropDownDataDTO>();
            var query = db.Items.ToList();

            foreach (var item in query)
            {
                if (ModelId == 0)
                {
                    var Obj = new DropDownDataDTO();
                    Obj.ID = item.ID;
                    Obj.Name = item.Name;
                    lst.Add(Obj);
                }
                else
                {
                    if (string.IsNullOrEmpty(item.CarModelsIDs) || item.CarModelsIDs == "null")
                    {
                        var Obj = new DropDownDataDTO();
                        Obj.ID = item.ID;
                        Obj.Name = item.Name;
                        lst.Add(Obj);
                    }
                    else
                    {
                        var list = item.CarModelsIDs.Split(',');
                        if (list.Contains(ModelId.ToString()))
                        {
                            var Obj = new DropDownDataDTO();
                            Obj.ID = item.ID;
                            Obj.Name = item.Name;
                            lst.Add(Obj);
                        }
                    }
                }
            
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkbalance(int ItemID)
        {

            var _item = db.Items.Where(x => x.ID == ItemID ).FirstOrDefault();
            if (_item == null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);

            }
          
            var _Store = db.StoresBalances.Where(x => x.ItemID == ItemID && x.StoreID == _sessionMange.StoreID).FirstOrDefault();
            if (_Store == null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {

                decimal balanceQty = ((decimal)_Store.Quantity);
                return Json(balanceQty, JsonRequestBehavior.AllowGet);

            }
        }


        public JsonResult GetItemPrice(int ItemID)
        {

            var _item = db.Items.Where(x => x.ID == ItemID).FirstOrDefault();
            if (_item == null)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(_item.Price, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

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

                var varData = db.Items.Select(x => new SearchVM { ID = x.ID, Name = x.Name, CategoryName = x.ItemCategory.Name , Price = x.Price }).ToList().AsEnumerable();
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

        public ActionResult CreateExcel()
        {

            var table = new System.Data.DataTable("ExceFile");


           
            var data = db.Items.Select(x => new SearchVM { ID = x.ID, Name = x.Name, CategoryName = x.ItemCategory.Name, Price = x.Price }).ToList().AsEnumerable();
            table.Columns.Add("الكود", typeof(int));
            table.Columns.Add("الاسم", typeof(string));
            table.Columns.Add("التصنيف", typeof(string));
            table.Columns.Add("سعر البيع", typeof(string));
            foreach (var item in data)
            {
                table.Rows.Add(item.ID, item.Name, item.CategoryName, item.Price);
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

        #region new
        int GetNewItemId()
        {

            int NewtypeId = 1;
            try
            {
                int? ItemID = db.Items.Max(p => p.ID);
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

            Item obj = new Item();
            ViewBag.CarModelsIDs = new SelectList(db.CarModels.ToList(), "ID", "Name");
            ViewBag.CategoryID = new SelectList(db.ItemCategories.ToList(), "ID", "Name");
            obj.ID = GetNewItemId();
            return View(obj);
        }
        [HttpPost]
        public JsonResult Create(Item MasterObj)
        {

            try
            {

                db.Items.Add(MasterObj);
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
            Item obj = db.Items.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.CarModelsIDs = new SelectList(db.CarModels.ToList(), "ID", "Name");
            ViewBag.CategoryID = new SelectList(db.ItemCategories.ToList(), "ID", "Name",obj.CategoryID);
            return View(obj);
        }
        [HttpPost]
        public JsonResult Edit(Item MasterObj)
        {

            try
            {

                Item _DbObj = new Item();
                _DbObj = db.Items.Where(x => x.ID == MasterObj.ID).FirstOrDefault();
                _DbObj.Name = MasterObj.Name;
                _DbObj.Price = MasterObj.Price;
                _DbObj.CarModelsIDs = MasterObj.CarModelsIDs;
                _DbObj.CategoryID = MasterObj.CategoryID;
                _DbObj.PurchasingPrice = MasterObj.PurchasingPrice;
                _DbObj.Notes = MasterObj.Notes;

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
                var _obj = db.Items.Where(X => X.ID == ID).FirstOrDefault();
                db.Items.Remove(_obj);
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





    }
}