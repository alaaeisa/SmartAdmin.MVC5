using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class SearchVM
    {
        public int ID { get;  set; }
        public string Name { get;  set; }
        public string Date { get; set; }
        public string Notes { get;  set; }
        public decimal Price { get;  set; }
        public string Phone { get;  set; }
        public decimal Quantity { get;  set; }
    }
}