using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class InvoiceItemVM
    {
        public int ID { get; set; }
        public int SerialNo { get; set; }
        public int InvoiceID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get;  set; }
        public decimal Qty { get; set; }
        public decimal ItemDiscount { get; set; }
        public decimal Price { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal ItemNet { get; set; }
        public InvoiceItemVM Convert(InvoiceItem DBObj)
        {
            InvoiceItemVM VMObj = new InvoiceItemVM();

            VMObj.ID = DBObj.ID;
            VMObj.SerialNo = DBObj.SerialNo;
            VMObj.InvoiceID = DBObj.InvoiceID;
            VMObj.ItemID = DBObj.ItemID;
            VMObj.ItemName = DBObj.Item.Name;
            VMObj.Qty = DBObj.Qty;
            VMObj.ItemDiscount = DBObj.ItemDiscount;
            VMObj.Price = DBObj.Price;
            VMObj.ItemTotal = DBObj.ItemTotal;
            VMObj.ItemNet = DBObj.ItemNet;

            return VMObj;
        }
    }
}