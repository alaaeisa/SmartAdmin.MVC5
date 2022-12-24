using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public  class DbLogs
    {
      public static  void logData(string _ControllerName, string _ActionName, string _msg, string _UserMsg)
        {
            //TrueDBEntities TrDB = new TrueDBEntities();
            //Log _logobj = new Log();
            //_logobj.CompanyID = int.Parse(Cookies.get_compId());
            //_logobj.ControllerName = _ControllerName;
            //_logobj.Date = DateTime.UtcNow.Date.ToString("yyyy/MM/dd");
            //_logobj.ActionName = _ActionName;
            //_logobj.Msg = _msg;
            //_logobj.UserNotes = _UserMsg;
            //TrDB.Logs.Add(_logobj);
            //TrDB.SaveChanges();

        }
    }
}