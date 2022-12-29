<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseReport.aspx.cs" Inherits="SmartAdminMvc.Report.BaseReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ClientIDMode="Static"  ID="rptViewer" runat="server"
                AutoDataBind="True" 
                HasCrystalLogo="False" HasRefreshButton="True" 
                EnableParameterPrompt="False" 
                allowDatabaseLogonPrompting="False" 
                HyperlinkTarget="_blank" PrintMode="ActiveX" />
        </div>
    </form>
</body>
</html>
