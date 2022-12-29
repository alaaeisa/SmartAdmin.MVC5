using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartAdminMvc.Report
{
    public partial class BaseReport : System.Web.UI.Page
    {
        private ReportClass rptH;
        SessionMange _sessionMange = new SessionMange();
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = _sessionMange;
            if (user.UserID == 0)
            {
                Response.Redirect("/Home/Index");
            }
            if (Request.QueryString["rpt"] != null)
            {
                string type = Request.QueryString["rpt"].ToString();
                //**********
                bool IsPDF = bool.Parse(Request.QueryString["GenType"].ToString());
                var Index = (ReportBL.ReportIndex)Session[type];
                if (Session["Rep_" + Index.ReportType] == null || !IsPostBack)
                {
                    var li = new ReportBL().RelodDataSource(Index);
                    var img = HttpContext.Current.Server.MapPath(user.Logo);
                    rptH = new ReportClass();
                    rptH.FileName = Server.MapPath(@"/Report/MainReports/" + li.FileName + "");
                    rptH.Load();
                    Session["Rep_" + Index.ReportType] = li;
                    rptH.SetDataSource(li.Data);
                    if (li.HasSubReport)
                    {
                        rptH.Subreports[0].SetDataSource(li.SubReportData);
                    }
                    rptH.SetParameterValue("CompanyName", user.CompanyName ?? "");
                    rptH.SetParameterValue("CompanyLogo", img ?? "");

                    if (IsPDF)
                    {
                        rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, li.FileName);
                    }
                    else
                    {
                        rptViewer.ReportSource = rptH;
                    }
                }
                else
                {
                    var li = (ReportBL)Session["Rep_" + Index.ReportType];
                    var img = HttpContext.Current.Server.MapPath(user.Logo);
                    rptH = new ReportClass();
                    rptH.FileName = Server.MapPath(@"/Report/MainReports/" + li.FileName + "");
                    rptH.Load();
                    rptH.SetDataSource(li.Data);
                    if (li.HasSubReport)
                    {
                        rptH.Subreports[0].SetDataSource(li.SubReportData);
                    }
                    rptH.SetParameterValue("CompanyName", user.CompanyName??"");
                    rptH.SetParameterValue("CompanyLogo", img);

                    if (IsPDF)
                    {
                        rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, li.FileName);
                    }
                    else
                    {
                        rptViewer.ReportSource = rptH;
                    }
                }
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            rptH.Close();
            rptH.Dispose();
        }
    }
}