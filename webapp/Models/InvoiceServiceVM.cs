using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class InvoiceServiceVM
    {
        public int ID { get; set; }
        public int SerialNo { get; set; }
        public int InvoiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal ServiceDiscount { get; set; }
        public decimal ServiceTotal { get; set; }
        public decimal ServiceNet { get; set; }
        public InvoiceServiceVM Convert(InvoiceService DBObj)
        {
            InvoiceServiceVM VMObj = new InvoiceServiceVM();

            VMObj.ID = DBObj.ID;
            VMObj.SerialNo = DBObj.SerialNo;
            VMObj.InvoiceID = DBObj.InvoiceID;
            VMObj.ServiceName = DBObj.ServiceName;
            VMObj.ServicePrice = DBObj.ServicePrice;
            VMObj.ServiceDiscount = DBObj.ServiceDiscount;
            VMObj.ServiceTotal = DBObj.ServiceTotal;
            VMObj.ServiceNet = DBObj.ServiceNet;

            return VMObj;
        }
    }
}