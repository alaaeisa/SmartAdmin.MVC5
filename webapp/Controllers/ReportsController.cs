//using CrystalDecisions.CrystalReports.Engine;
//using SmartAdminMvc.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;


//namespace SmartAdminMvc.Controllers
//{
//    public class ReportsController : Controller
//    {
//        SessionMange _sessionMange = new SessionMange();
//        TrueDBEntities db = new TrueDBEntities();
//      //  GET: Reports
//        public ActionResult Index()
//        {
//            ViewBag.SupplierID = new SelectList(db.Suppliers.Select(x => new { x.SupplierID, x.SupplierName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "SupplierID", "SupplierName");
//            ViewBag.CustomerId = new SelectList(db.Customers.Select(x => new { x.CustomerId, x.CustomerName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "CustomerId", "CustomerName");
//            ViewBag.Items = new SelectList(db.Items.Select(x => new { x.ItemID, x.ItemName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "ItemID", "ItemName");
//            ViewBag.Units = new SelectList(db.Measurementunits.Select(x => new { x.nUnitID, x.sUnitName, x.CompanyID }).Where(x => x.CompanyID == _sessionMange.CompId).ToList(), "nUnitID", "sUnitName");
//            return View();
//        }

//        #region Sales
//        public ActionResult PrintSalesRep(string FromDate, string ToData, int CustomerId = 0)
//        {
//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }

//            List<Vuw_SalesInvoiceRep> AllList = new List<Vuw_SalesInvoiceRep>();
//            var _lstdata = db.Vuw_SalesInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId).ToList();
//            if (CustomerId != 0)
//            {
//                _lstdata = db.Vuw_SalesInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId && x.CustomerId == CustomerId).ToList();
//            }
//            DateTime start = Convert.ToDateTime(FromDate);
//            DateTime end = Convert.ToDateTime(ToData);

//            foreach (var item in _lstdata)
//            {
//                DateTime _date = Convert.ToDateTime(item.InvoiceDate);
//                if (_date >= start && _date <= end)
//                {
//                    AllList.Add(item);
//                }
//            }
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "SalesInVoice.rpt"));
//            var ds = (from p in AllList
//                      select new
//                      {

//                          SCustomerName = p.SCustomerName ?? "",
//                          CustomerId = p.CustomerId,

//                          InvoiceNo = p.InvoiceNo,
//                          InvoiceDate = p.InvoiceDate ?? "",
//                          CustomerName = p.CustomerName ?? "",
//                          CustomerPhone = p.CustomerPhone ?? "",
//                          CustomerAddress = p.CustomerAddress ?? "",
//                          net = p.net ?? 0,
//                          Total = p.Total ?? 0,
//                          Notes = p.Notes ?? "",
//                          CompanyName = p.CompanyName ?? "",
//                          CompanyID = p.CompanyID ?? 0,
//                          ItemID = p.ItemID ?? 0,
//                          Qty = p.Qty ?? 0,
//                          ItemTotal = p.ItemTotal ?? 0,
//                          Price = p.Price ?? 0,
//                          ItemDiscount = p.ItemDiscount ?? 0,
//                          ItemTax = p.ItemTax ?? 0,
//                          ItemNet = p.ItemNet ?? 0,
//                          nUnitID = p.nUnitID ?? 0,

//                          sUnitName = p.sUnitName ?? "",
//                          ItemName = p.ItemName ?? "",
//                          Notes = p.Notes ?? "",

//                      }).ToList();
//            rd.SetDataSource(ds);
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
//                return Content("عذرا  التقارير  مفعله على  النسخه المدفوعه فقط ");
//                throw ex;
//            }
//        }




//        public ActionResult PrintRetuenSaleREP(string FromDate, string ToData, int CustomerId = 0)
//        {
//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }

//            List<Vuw_ReturnSalesInvoiceRep> AllList = new List<Vuw_ReturnSalesInvoiceRep>();

//            var _lstdata = db.Vuw_ReturnSalesInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId).ToList();
//            if (CustomerId != 0)
//            {
//                _lstdata = db.Vuw_ReturnSalesInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId && x.CustomerId == CustomerId).ToList();
//            }
//            DateTime start = Convert.ToDateTime(FromDate);
//            DateTime end = Convert.ToDateTime(ToData);

//            foreach (var item in _lstdata)
//            {
//                DateTime _date = Convert.ToDateTime(item.InvoiceDate);
//                if (_date >= start && _date <= end)
//                {
//                    AllList.Add(item);
//                }
//            }
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReturnSalesInVoice.rpt"));
//            var ds = (from p in AllList
//                      select new
//                      {

//                          SCustomerName = p.SCustomerName ?? "",
//                          CustomerId = p.CustomerId,

//                          InvoiceNo = p.InvoiceNo,
//                          InvoiceDate = p.InvoiceDate ?? "",
//                          CustomerName = p.CustomerName ?? "",
//                          CustomerPhone = p.CustomerPhone ?? "",
//                          CustomerAddress = p.CustomerAddress ?? "",
//                          net = p.net ?? 0,
//                          Total = p.Total ?? 0,
//                          Notes = p.Notes ?? "",
//                          CompanyName = p.CompanyName ?? "",
//                          CompanyID = p.CompanyID ?? 0,
//                          ItemID = p.ItemID ?? 0,
//                          Qty = p.Qty ?? 0,
//                          ItemTotal = p.ItemTotal ?? 0,
//                          Price = p.Price ?? 0,
//                          ItemDiscount = p.ItemDiscount ?? 0,
//                          ItemTax = p.ItemTax ?? 0,
//                          ItemNet = p.ItemNet ?? 0,
//                          nUnitID = p.nUnitID ?? 0,

//                          sUnitName = p.sUnitName ?? "",
//                          ItemName = p.ItemName ?? "",
//                          Notes = p.Notes ?? "",

//                      }).ToList();
//            rd.SetDataSource(ds);
//            Response.Buffer = false;
//            Response.ClearContent();
//            Response.ClearHeaders();
//            try
//            {
//                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                stream.Seek(0, SeekOrigin.Begin);
//                return File(stream, "application/pdf", "ReturnSalesInVoice.pdf");
//            }
//            catch (Exception ex)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//                throw ex;
//            }
//        }
//        #endregion
//        #region PURchase
//        public ActionResult PrintPurchaseRep(string FromDate, string ToData, int SupplierID = 0)
//        {
//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }

//            List<Vuw_PurchaseInvoiceRep> AllList = new List<Vuw_PurchaseInvoiceRep>();
//            var _lstdata = db.Vuw_PurchaseInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId).ToList();
//            if (SupplierID != 0)
//            {
//                _lstdata = db.Vuw_PurchaseInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId && x.SupplierID == SupplierID).ToList();
//            }

//            DateTime start = Convert.ToDateTime(FromDate);
//            DateTime end = Convert.ToDateTime(ToData);

//            foreach (var item in _lstdata)
//            {
//                DateTime _date = Convert.ToDateTime(item.InvoiceDate);
//                if (_date >= start && _date <= end)
//                {
//                    AllList.Add(item);
//                }
//            }
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "PurchaseInVoice.rpt"));
//            var ds = (from p in _lstdata
//                      select new
//                      {

//                          SSupplierName = p.SSupplierName ?? "",
//                          SupplierID = p.SupplierID,

//                          InvoiceNo = p.InvoiceNo,
//                          InvoiceDate = p.InvoiceDate ?? "",
//                          SupplierName = p.SupplierName ?? "",
//                          SupplierPhone = p.SupplierPhone ?? "",
//                          SupplierAddress = p.SupplierAddress ?? "",
//                          net = p.net ?? 0,
//                          Total = p.Total ?? 0,
//                          Notes = p.Notes ?? "",
//                          CompanyName = p.CompanyName ?? "",
//                          CompanyID = p.CompanyID ?? 0,
//                          ItemID = p.ItemID ?? 0,
//                          Qty = p.Qty ?? 0,
//                          ItemTotal = p.ItemTotal ?? 0,
//                          Price = p.Price ?? 0,
//                          ItemDiscount = p.ItemDiscount ?? 0,
//                          ItemTax = p.ItemTax ?? 0,
//                          ItemNet = p.ItemNet ?? 0,
//                          nUnitID = p.nUnitID ?? 0,

//                          sUnitName = p.sUnitName ?? "",
//                          ItemName = p.ItemName ?? "",

//                      }).ToList();
//            rd.SetDataSource(ds);
//            Response.Buffer = false;
//            Response.ClearContent();
//            Response.ClearHeaders();
//            try
//            {
//                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                stream.Seek(0, SeekOrigin.Begin);
//                return File(stream, "application/pdf", "PurchaseInVoice.pdf");
//            }
//            catch (Exception ex)
//            {
//                return Content("<h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//                throw ex;
//            }
//        }



//        public ActionResult PrintReturnPurchaseRep(string FromDate, string ToData, int SupplierID = 0)
//        {
//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }
//            List<Vuw_ReturnPurchaseInvoiceRep> AllList = new List<Vuw_ReturnPurchaseInvoiceRep>();
//            var _lstdata = db.Vuw_ReturnPurchaseInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId).ToList();
//            if (SupplierID != 0)
//            {
//                _lstdata = db.Vuw_ReturnPurchaseInvoiceRep.Where(x => x.CompanyID == _sessionMange.CompId && x.SupplierID == SupplierID).ToList();
//            }
//            DateTime start = Convert.ToDateTime(FromDate);
//            DateTime end = Convert.ToDateTime(ToData);

//            foreach (var item in _lstdata)
//            {
//                DateTime _date = Convert.ToDateTime(item.InvoiceDate);
//                if (_date >= start && _date <= end)
//                {
//                    AllList.Add(item);
//                }
//            }
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReturenPurchaseInVoice.rpt"));
//            var ds = (from p in _lstdata
//                      select new
//                      {

//                          SSupplierName = p.SSupplierName ?? "",
//                          SupplierID = p.SupplierID,

//                          InvoiceNo = p.InvoiceNo,
//                          InvoiceDate = p.InvoiceDate ?? "",
//                          SupplierName = p.SupplierName ?? "",
//                          SupplierPhone = p.SupplierPhone ?? "",
//                          SupplierAddress = p.SupplierAddress ?? "",
//                          net = p.net ?? 0,
//                          Total = p.Total ?? 0,
//                          Notes = p.Notes ?? "",
//                          CompanyName = p.CompanyName ?? "",
//                          CompanyID = p.CompanyID ?? 0,
//                          ItemID = p.ItemID ?? 0,
//                          Qty = p.Qty ?? 0,
//                          ItemTotal = p.ItemTotal ?? 0,
//                          Price = p.Price ?? 0,
//                          ItemDiscount = p.ItemDiscount ?? 0,
//                          ItemTax = p.ItemTax ?? 0,
//                          ItemNet = p.ItemNet ?? 0,
//                          nUnitID = p.nUnitID ?? 0,

//                          sUnitName = p.sUnitName ?? "",
//                          ItemName = p.ItemName ?? "",

//                      }).ToList();
//            rd.SetDataSource(ds);
//            Response.Buffer = false;
//            Response.ClearContent();
//            Response.ClearHeaders();
//            try
//            {
//                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                stream.Seek(0, SeekOrigin.Begin);
//                return File(stream, "application/pdf", "ReturenPurchaseInVoice.pdf");
//            }
//            catch (Exception ex)
//            {
//                return Content("<h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//                throw ex;
//            }
//        }

//        #endregion

//        #region    items

//        public ActionResult PrintItemsRep(string FromDate, string ToData, int ItemId = 0, int UnitId = 0)
//        {

//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }
//        }


//        #endregion

//        #region    Suppliers 

//        public ActionResult PrintSuppRep(string FromDate, string ToData, int SupplierID = 0)
//        {

//            return Content("<h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//        }


//        #endregion

//        #region    Customers

//        public ActionResult PrintCustRep(string FromDate, string ToData, int CustomerId = 0)
//        {

//            if (1 == 1)
//            {
//                return Content("<br/> <br/><h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//            }
//        }


//        #endregion

//        public ActionResult printItemsReport()
//        {
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ItemReport.rpt"));
//            var lst = db.Items.Where(x => x.CompanyID == _sessionMange.CompId).Select(p => new
//            {
//                ItemName = p.ItemName,
//                ItemID = p.ItemID,
//                PurchasePrice = p.PurchasePrice ?? 0,
//                Price = p.Price ?? 0,
//                Notes = p.Notes,

//            }).ToList();
//            rd.SetDataSource(lst);
//            Response.Buffer = false;
//            Response.ClearContent();
//            Response.ClearHeaders();
//            try
//            {
//                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                stream.Seek(0, SeekOrigin.Begin);
//                return File(stream, "application/pdf", "ItemReport.pdf");
//            }
//            catch (Exception ex)
//            {
//                return Content("<h1>عذرا  التقارير  مفعله على  النسخه المدفوعه فقط</h1>  ");
//                throw ex;
//            }



//        }
//        public ActionResult printItemsQTyReport()
//        {
//            ReportDocument rd = new ReportDocument();
//            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ItemQtyReport.rpt"));
//            var lst = db.Vuw_ItemQty.Where(x => x.CompanyID == _sessionMange.CompId).Select(p => new
//            {
//                ItemName = p.ItemName,
//                ItemID = p.ItemID,
//                Quantity = p.Quantity ?? 0,
//            }).ToList();
//            rd.SetDataSource(lst);
//            Response.Buffer = false;
//            Response.ClearContent();
//            Response.ClearHeaders();
//            try
//            {
//                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                stream.Seek(0, SeekOrigin.Begin);
//                return File(stream, "application/pdf", "ItemQtyReport.pdf");
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }




//        public ActionResult printRestorationReport(string Start, string End)
//        {
//            string[] StartDate = (Start.ToString()).Split('.');
//            string _sdate = StartDate[2] + "/" + StartDate[1] + "/" + StartDate[0];
//            string[] EndDate = (End.ToString()).Split('.');
//            string _edate = EndDate[2] + "/" + EndDate[1] + "/" + EndDate[0];
//            DateTime start = DateFormater.ToDateTime(_sdate);
//            DateTime end = DateFormater.ToDateTime(_edate);
//            int SectionID = GetSectionID();
//            if (SectionID == 1)
//            {
//                // var permits = db.VuwRestorationPermitReports.Select(x => x).ToList();
//                var permits = db.VuwRestorationPermitReports.ToList();

//                List<VuwRestorationPermitReport> AllList = new List<VuwRestorationPermitReport>();

//                foreach (var item in permits)
//                {


//                    string[] DateVisit = (item.start_date.ToString()).Split('.');
//                    string _DateVisit = DateVisit[2] + "/" + DateVisit[1] + "/" + DateVisit[0];
//                    DateTime Visit = DateFormater.ToDateTime(_DateVisit);

//                    if (Visit >= start && Visit <= end)
//                    {
//                        AllList.Add(item);
//                    }
//                }


//                ReportDocument rd = new ReportDocument();
//                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RestorationPermit.rpt"));
//                rd.SetDataSource(AllList.Distinct());
//                Response.Buffer = false;
//                Response.ClearContent();
//                Response.ClearHeaders();
//                try
//                {
//                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                    stream.Seek(0, SeekOrigin.Begin);
//                    return File(stream, "application/pdf", "RestorationPermit.pdf");
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }

//            }
//            else
//            {
//                ReportDocument rd = new ReportDocument();
//                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RestorationPermit.rpt"));
//                var permits = db.VuwRestorationPermitReports.Select(x => x).Where(x => x.n_section_id == SectionID).ToList();
//                List<VuwRestorationPermitReport> AllList = new List<VuwRestorationPermitReport>();
//                foreach (var item in permits)
//                {
//                    string[] DateVisit = (item.start_date.ToString()).Split('.');
//                    string _DateVisit = DateVisit[2] + "/" + DateVisit[1] + "/" + DateVisit[0];
//                    DateTime Visit = DateFormater.ToDateTime(_DateVisit);

//                    if (Visit >= start && Visit <= end)
//                    {
//                        AllList.Add(item);
//                    }
//                }
//                rd.SetDataSource(AllList);
//                Response.Buffer = false;
//                Response.ClearContent();
//                Response.ClearHeaders();
//                try
//                {
//                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//                    stream.Seek(0, SeekOrigin.Begin);
//                    return File(stream, "application/pdf", "RestorationPermit.pdf");
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }

//    }
//}