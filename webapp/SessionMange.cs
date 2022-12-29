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
                
                 var _UserID = Cookies.get_UserID();
                if (!string.IsNullOrEmpty(_UserID) && _UserID != "0" )
                {
                    return int.Parse(_UserID);
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
                var _StoreID = Cookies.get_StoreID();
                if (!string.IsNullOrEmpty(_StoreID) && _StoreID != "0")
                {
                    return int.Parse(_StoreID);
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

        public string UserName { get; set; }
        public bool Authorize { get; set; }
        public bool BigAdmin { get; set; }
   

        public string CompanyName
        {
            get
            {
                var _CompanyName = Cookies.getCompName();
                if (!string.IsNullOrEmpty(_CompanyName) && _CompanyName != "0")
                {
                    return HttpUtility.UrlDecode(_CompanyName);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(_loginUrl, true);
                    return "";
                }

            }
            set
            {
                if (value !="")
                {
                    Cookies.setCompName(value.ToString());
                }
            }
        }

        public string Logo
        {
            get
            {
                var _Logo = Cookies.getCompLogo();
                if (!string.IsNullOrEmpty(_Logo) && _Logo != "0")
                {
                    return _Logo;
                }
                else
                {
                    HttpContext.Current.Response.Redirect(_loginUrl, true);
                    return "";
                }

            }
            set
            {
                if (value != "")
                {
                    Cookies.setCompLogo(value.ToString());
                }
            }
        }


    }
}