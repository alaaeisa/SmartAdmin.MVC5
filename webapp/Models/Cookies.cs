using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class Cookies
    {
       

        public static void setCompName(string val)
        {
            HttpContext.Current.Response.Cookies.Remove("CompName");
            HttpCookie cookie = new HttpCookie("CompName");
            cookie["name"] = HttpUtility.UrlEncode(val);
            cookie.Expires = DateTime.Now.AddDays(30d);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string getCompName()
        {
            string name = string.Empty;
            HttpCookie langauge = System.Web.HttpContext.Current.Request.Cookies.Get("CompName");
            if (langauge == null) return "0";
            name = langauge["name"];
            return name;
        }

        public static void set_UserName(string val)
        {
            HttpContext.Current.Response.Cookies.Remove("UserName");
            HttpCookie cookie = new HttpCookie("UserName");
            cookie["name"] = HttpUtility.UrlEncode(val);
            cookie.Expires = DateTime.Now.AddDays(30d);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string get_UserName()
        {
            string name = string.Empty;
            HttpCookie langauge = System.Web.HttpContext.Current.Request.Cookies.Get("UserName");
            if (langauge == null) return "0";
            name = langauge["name"];
            return name;
        }


        public static void set_UserAuthorize(string val)
        {
            HttpContext.Current.Response.Cookies.Remove("UserAuthorize");
            HttpCookie cookie = new HttpCookie("UserAuthorize");
            cookie["name"] = val;
            cookie.Expires = DateTime.Now.AddDays(30d);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string get_UserAuthorize()
        {
            string name = string.Empty;
            HttpCookie langauge = System.Web.HttpContext.Current.Request.Cookies.Get("UserAuthorize");
            if (langauge == null) return "0";
            name = langauge["name"];
            return name;
        }
        

          public static void set_StoreID(string val)
        {
            HttpContext.Current.Response.Cookies.Remove("StoreID");
            HttpCookie cookie = new HttpCookie("StoreID");
            cookie["name"] = val;
            cookie.Expires = DateTime.Now.AddDays(30d);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string get_StoreID()
        {
            string name = string.Empty;
            HttpCookie langauge = System.Web.HttpContext.Current.Request.Cookies.Get("StoreID");
            if (langauge == null) return "0";
            name = langauge["name"];
            return name;
        }

        public static void set_UserID(string val)
        {
            HttpContext.Current.Response.Cookies.Remove("UserID");
            HttpCookie cookie = new HttpCookie("UserID");
            cookie["name"] = val;
            cookie.Expires = DateTime.Now.AddDays(30d);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string get_UserID()
        {
            string name = string.Empty;
            HttpCookie langauge = System.Web.HttpContext.Current.Request.Cookies.Get("UserID");
            if (langauge == null) return "0";
            name = langauge["name"];
            return name;
        }

        public static void set_SuperAdmin(string val)
        {
            HttpContext.Current.Response.Cookies.Remove("SuperAdmin");
            HttpCookie cookie = new HttpCookie("SuperAdmin");
            cookie["name"] = val;
            cookie.Expires = DateTime.Now.AddDays(30d);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string get_SuperAdmin()
        {
            string name = string.Empty;
            HttpCookie langauge = System.Web.HttpContext.Current.Request.Cookies.Get("SuperAdmin");
            if (langauge == null) return "0";
            name = langauge["name"];
            return name;
        }


    }
}