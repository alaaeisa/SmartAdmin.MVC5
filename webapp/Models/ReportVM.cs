using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class ReportVM
    {
        public List<DropDownDataDTO> Customers { get; set; }
        public List<DropDownDataDTO> Items { get; set; }
        public List<DropDownDataDTO> ItemsCategories { get; set; }
        public List<DropDownDataDTO> Stores { get; set; }
        public ReportVM(string type)
        {
            using (var db = new CarWorkShopEntities())
            {
                if (type == "InvoiceMaster" || type == "InvoiceDetails")
                {
                    Customers = db.Customers.Select(x => new DropDownDataDTO()
                    {
                        ID = x.ID,
                        Name = x.Name,
                    }).ToList();

                }
                if (type == "InvoiceDetails" || type == "ItemsBalance")
                {
                    Items = db.Items.Select(x => new DropDownDataDTO()
                    {
                        ID = x.ID,
                        Name = x.Name,
                    }).ToList();

                    ItemsCategories = db.ItemCategories.Select(x => new DropDownDataDTO()
                    {
                        ID = x.ID,
                        Name = x.Name,
                    }).ToList();
                    if (type == "ItemsBalance")
                    {
                        Stores = db.Stores.Select(x => new DropDownDataDTO()
                        {
                            ID = x.ID,
                            Name = x.Name,
                        }).ToList();
                    }
                }
            }
        }
    }
}