using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class CarBrandsController : Controller
    {
        // GET: CarBrands
        NotificationMessage msg = new NotificationMessage();
        SessionMange _sessionMange = new SessionMange();
        CarWorkShopEntities db = new CarWorkShopEntities();
        string _ControllerName = "CarBrands";
        

        public JsonResult GetModelData(int BrandId)
        {

            var lst = new List<DropDownDataDTO>();
            var query = db.CarModels.Where(x=>x.BrandId == BrandId).ToList();

            foreach (var item in query)
            {
               
                    var Obj = new DropDownDataDTO();
                    Obj.ID = item.ID;
                    Obj.Name = item.Name;
                    lst.Add(Obj);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
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

                var varData = db.CarBrands.Select(x => new SearchVM { ID = x.ID, Name = x.Name, Notes = x.Notes }).ToList().AsEnumerable();
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
                int? ItemID = db.CarBrands.Max(p => p.ID);
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

            CarBrand obj = new CarBrand();
            obj.ID = GetNewId();
            return View(obj);
        }
        [HttpPost]
        public JsonResult Create(CarBrand MasterObj)
        {

            try
            {

                db.CarBrands.Add(MasterObj);
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
            CarBrand obj = db.CarBrands.Where(x => x.ID == id).FirstOrDefault();
            return View(obj);
        }
        [HttpPost]
        public JsonResult Edit(Item MasterObj)
        {
            try
            {
                CarBrand _DbObj = new CarBrand();
                _DbObj = db.CarBrands.Where(x => x.ID == MasterObj.ID).FirstOrDefault();
                _DbObj.Name = MasterObj.Name;
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
                var _obj = db.CarBrands.Where(X => X.ID == ID).FirstOrDefault();
                db.CarBrands.Remove(_obj);
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