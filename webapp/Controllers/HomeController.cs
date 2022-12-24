#region Using

using SmartAdminMvc.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{
    //[Authorize]
    public class dashboard
    {
        public int custNo { get; set; }
        public int billNo { get; set; }
        public int supNo { get; set; }
        public decimal cashbalance { get; set; }
        public int itemNo { get; set; }
    }
    public class HomeController : Controller
    {
        CarWorkShopEntities db = new CarWorkShopEntities();
        SessionMange _sessionMange = new SessionMange();
        // GET: home/index
        string _ControllerName = "home";
        public ActionResult Index()
        {
            try
            {
                dashboard _obj = new dashboard();
                _obj.billNo = db.InvoiceMasters.Count();
                _obj.itemNo = db.Items.Count();
                _obj.custNo = db.Customers.Count();

                return View(_obj);
            }
            catch (System.Exception ex)
            {

                DbLogs.logData(_ControllerName, "Delete", ex.Message, "    ");
                return Content("Error");
            }
            

           
           
        }

        public ActionResult Social()
        {
            return View();
        }


        public ActionResult lol()
        {
            return View();
        }



        // GET: home/inbox
        public ActionResult Inbox()
        {
            return View();
        }

        // GET: home/widgets
        public ActionResult Widgets()
        {
            return View();
        }

        // GET: home/chat
        public ActionResult Chat()
        {
            return View();
        }
    }
}