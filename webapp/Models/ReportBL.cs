using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class ReportBL
    {
        public class ReportIndex
        {
            public string ReportType { get; set; }
            public string FDate { get; set; }
            public string ToDate { get; set; }
            public int? InvoiceID { get; set; }
            public int? CustomerID { get; set; }
            public int? ItemID { get; set; }
            public int? ItemGroupID { get; set; }
            public int? StoreID { get; set; }
        }
        public string FileName { get; set; }
        public object Data { get; set; }
        public object SubReportData { get; set; }
        public string SessionName { get; set; }
        public bool HasSubReport { get; set; }

        public ReportBL RelodDataSource(ReportIndex Index)
        {
            var Obj = new ReportBL();
            using (var db = new CarWorkShopEntities())
            {
                if (Index.ReportType == "MsInvoice")
                {
                    Obj.SessionName = "Rep_1";
                    Obj.FileName = "InvoiceMaster.rpt" ;
                    Obj.Data = db.GetMaintainceOrder(Index.InvoiceID).ToList();
                }
                else if (Index.ReportType == "DtInvoice")
                {
                    var FromDate = string.IsNullOrEmpty(Index.FDate) ? null : Index.FDate;
                    var TDate = string.IsNullOrEmpty(Index.ToDate) ? null : Index.ToDate;
                    Obj.SessionName = "Rep_2";
                    Obj.FileName =  "InvoiceDetails.rpt";
                    Obj.HasSubReport = true;
                    Obj.Data = db.GetInvoiceDetailsWithItems(Index.InvoiceID).ToList();
                    Obj.SubReportData = db.GetInvoiceDetailsWithService(Index.InvoiceID).ToList();
                }
                else if (Index.ReportType == "MoInDate")
                {
                    var FromDate = string.IsNullOrEmpty(Index.FDate) ? null : Index.FDate;
                    var TDate = string.IsNullOrEmpty(Index.ToDate) ? null : Index.ToDate;
                    Obj.SessionName = "Rep_3";
                    Obj.FileName = "MaintanceOrdersInDate.rpt";
                    Obj.Data = db.GetInvoicesInDate(Index.CustomerID, FromDate, TDate).ToList();
                    if ((Obj.Data as List<GetInvoicesInDate_Result>).Count == 0)
                        (Obj.Data as List<GetInvoicesInDate_Result>).Add(new GetInvoicesInDate_Result() { FromDate = Index.FDate, TromDate = Index.ToDate });
                }
                else if (Index.ReportType == "MoDetailsInDate")
                {
                    var FromDate = string.IsNullOrEmpty(Index.FDate) ? null : Index.FDate;
                    var TDate = string.IsNullOrEmpty(Index.ToDate) ? null : Index.ToDate;
                    Obj.SessionName = "Rep_4";
                    Obj.FileName = "MaintanceOrdersDetailsInDate.rpt";
                    Obj.Data = db.GetInvoicesDetailsInDate(Index.CustomerID, Index.ItemID,Index.ItemGroupID, FromDate, TDate).ToList();
                    if ((Obj.Data as List<GetInvoicesDetailsInDate_Result>).Count == 0)
                        (Obj.Data as List<GetInvoicesDetailsInDate_Result>).Add(new GetInvoicesDetailsInDate_Result() { FromDate = Index.FDate, TromDate = Index.ToDate });
                }
                else if (Index.ReportType == "ItemsBalanceInDate")
                {
                    var FromDate = string.IsNullOrEmpty(Index.FDate) ? null : Index.FDate;
                    var TDate = string.IsNullOrEmpty(Index.ToDate) ? null : Index.ToDate;
                    Obj.SessionName = "Rep_5";
                    Obj.FileName = "ItemsBalanceInDate.rpt" ;
                    Obj.Data = db.GetItemsBalanceInDate(Index.StoreID, Index.ItemID, Index.ItemGroupID, FromDate, TDate).ToList();
                    if ((Obj.Data as List<GetItemsBalanceInDate_Result>).Count == 0)
                        (Obj.Data as List<GetItemsBalanceInDate_Result>).Add(new GetItemsBalanceInDate_Result() { FromDate = Index.FDate, TromDate = Index.ToDate, CurrentQty = 0, ItemPrice = 0 });
                }
                else
                {

                }
            }

            return Obj;
        }
    }
}