//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartAdminMvc.Models
{
    using System;
    
    public partial class GetInvoiceDetailsWithService_Result
    {
        public int InvoiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal ServiceTotal { get; set; }
        public decimal ServiceDiscount { get; set; }
        public decimal ServiceNet { get; set; }
    }
}
