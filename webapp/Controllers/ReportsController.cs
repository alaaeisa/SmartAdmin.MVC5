using CrystalDecisions.CrystalReports.Engine;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SmartAdminMvc.Controllers
{
    public class ReportsController : Controller
    {
        SessionMange _sessionMange = new SessionMange();
        public ActionResult InvoicesInDate()
        {
            var model = new ReportVM("InvoiceMaster");
            return View(model);
        }
        public ActionResult InvoicesDetailsInDate()
        {
            var model = new ReportVM("InvoiceDetails");
            return View(model);
        }
        public ActionResult ItemsBalance()
        {
            var model = new ReportVM("ItemsBalance");
            return View(model);
        }
        public ActionResult FillDS(ReportBL.ReportIndex Index)
        {
            Session[Index.ReportType] = null;
            Session[Index.ReportType] = Index;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}