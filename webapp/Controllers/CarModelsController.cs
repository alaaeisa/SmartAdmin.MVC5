using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SmartAdminMvc.Controllers
{
    public class CarModelsController : Controller
    {
        // GET: CarModels

        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        string _ControllerName = "CarModels";

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

                var varData = db.CarModels.Select(x => new SearchVM { ID = x.ID, Name = x.Name, BrandName =  x.CarBrand.Name ,Notes = x.Notes }).ToList().AsEnumerable();
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
        #endregion

        #region new
        int GetNewId()
        {

            int NewtypeId = 1;
            try
            {
                int? ItemID = db.CarModels.Max(p => p.ID);
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

            CarModel obj = new CarModel();
            obj.ID = GetNewId();
            ViewBag.BrandId = new SelectList(db.CarBrands.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name");
           
            return View(obj);
        }
        [HttpPost]
        public JsonResult Create(CarModel MasterObj)
        {

            try
            {

                db.CarModels.Add(MasterObj);
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
            CarModel obj = db.CarModels.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.BrandId = new SelectList(db.CarBrands.Select(x => new { x.ID, x.Name }).ToList(), "ID", "Name", obj.BrandId);
            return View(obj);
        }
        [HttpPost]
        public JsonResult Edit(CarModel MasterObj)
        {
            try
            {
                CarModel _DbObj = new CarModel();
                _DbObj = db.CarModels.Where(x => x.ID == MasterObj.ID).FirstOrDefault();
                _DbObj.Name = MasterObj.Name;
                _DbObj.Notes = MasterObj.Notes;
                _DbObj.BrandId = MasterObj.BrandId;

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
                var _obj = db.CarModels.Where(X => X.ID == ID).FirstOrDefault();
                db.CarModels.Remove(_obj);
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

        public ActionResult CreateExcel()
        {

            var table = new System.Data.DataTable("ExceFile");


          
            var data = db.CarModels.Select(x => new SearchVM { ID = x.ID, Name = x.Name, BrandName = x.CarBrand.Name, Notes = x.Notes }).ToList().AsEnumerable();
            table.Columns.Add("الكود", typeof(int));
            table.Columns.Add("الاسم", typeof(string));
            table.Columns.Add("الماركة", typeof(string));
            table.Columns.Add("الملاحظات", typeof(string));
            foreach (var item in data)
            {
                table.Rows.Add(item.ID, item.Name,item.BrandName, item.Notes);
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
    }
}