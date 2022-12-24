using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc
{
    public class SessionMange
    {
        string _loginUrl = "/users/UserLogin";
       
        public int UserID
        {
            get
            {
                if (Cookies.get_UserID() != null || Cookies.get_UserID() != "0")
                {
                    return int.Parse(Cookies.get_UserID());
                }

                else
                {
                    HttpContext.Current.Response.Redirect(_loginUrl, true);
                    return 0;
                }

            }
            set
            {
                if (value > 0)
                {
                    Cookies.set_UserID(value.ToString());
                }
            }
        }


        public int StoreID
        {
            get
            {
                if (Cookies.get_StoreID() != null || Cookies.get_StoreID() != "0")
                {
                    return int.Parse(Cookies.get_StoreID());
                }
                else
                {
                    HttpContext.Current.Response.Redirect(_loginUrl, true);
                    return 0;
                }

            }
            set
            {
                if (value > 0)
                {
                    Cookies.set_StoreID(value.ToString());
                }
            }
        }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public bool Authorize { get; set; }
        public bool BigAdmin { get; set; }

        

    }
}