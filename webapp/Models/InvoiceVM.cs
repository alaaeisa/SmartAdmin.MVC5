using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class InvoiceVM
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string DateStr { get;  set; }
        public int CustomerId { get; set; }
        public string CustomerName { get;  set; }
        public string Notes { get; set; }
    
        public string ChassisNo { get; set; }
        public string PlateNumber { get; set; }
        public int? BrandId { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get;  set; }
        public int km { get; set; }
        public decimal Total { get; set; }
        public decimal NetAmount { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal InvoiceTax { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal NetAmountWithOutTax { get;  set; }
        public decimal RemainingAmount { get; set; }

     

        public string InvoiceItems { get; set; }
        public string  InvoiceServices { get; set; }
        public List<InvoiceItemVM> Items { get;  set; }
        public List<InvoiceServiceVM> Services { get; set; }
       

        public InvoiceVM Convert(InvoiceMaster DBObj , bool MasterOnly =false)
        {
            InvoiceVM VMObj = new InvoiceVM();

            VMObj.ID = DBObj.ID;
            VMObj.Date = DBObj.Date;
            VMObj.DateStr = DBObj.Date.ToString("yyyy/MM/dd");
            
            VMObj.CustomerId = DBObj.CustomerId;
            VMObj.CustomerName = DBObj.Customer.Name;
            VMObj.Notes = DBObj.Notes;
        
            VMObj.ChassisNo = DBObj.ChassisNo;
            VMObj.PlateNumber = DBObj.PlateNumber;
            VMObj.BrandId = DBObj.BrandId;
            VMObj.ModelId = DBObj.ModelId;
            VMObj.ModelName = DBObj.CarModel.Name;
            VMObj.km = DBObj.km;
            VMObj.Total = DBObj.Total;
            VMObj.NetAmount = DBObj.NetAmount;
            VMObj.InvoiceDiscount = DBObj.InvoiceDiscount;
            VMObj.TotalDiscount = DBObj.TotalDiscount;
            VMObj.InvoiceTax = DBObj.InvoiceTax;
            VMObj.PaidAmount = DBObj.PaidAmount;
            VMObj.NetAmountWithOutTax = DBObj.NetAmountWithOutTax;
            VMObj.RemainingAmount = DBObj.RemainingAmount;
            if (!MasterOnly)
            {
                if (DBObj.InvoiceItems != null)
                {
                    VMObj.Items = DBObj.InvoiceItems.Select(x => new InvoiceItemVM().Convert(x)).OrderBy(x=>x.SerialNo).ToList() ;


                }
                if (DBObj.InvoiceServices != null)
                {
                    VMObj.Services = DBObj.InvoiceServices.Select(x => new InvoiceServiceVM().Convert(x)).OrderBy(x => x.SerialNo).ToList();

                }
            }
          

            return VMObj;
        }
    }
}