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
            CarWorkShopEntities TrDB = new CarWorkShopEntities();
            DBLog _logobj = new DBLog();
            _logobj.StoreID = int.Parse(Cookies.get_StoreID());
            _logobj.ControllerName = _ControllerName;
            _logobj.Date = DateTime.UtcNow.Date;
            _logobj.ActionName = _ActionName;
            _logobj.EXMsg = _msg;
            _logobj.UserMsg = _UserMsg;
            TrDB.DBLogs.Add(_logobj);
            TrDB.SaveChanges();

        }
    }
}