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
    using System.Collections.Generic;
    
    public partial class InvoiceMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceMaster()
        {
            this.InvoiceItems = new HashSet<InvoiceItem>();
            this.InvoiceServices = new HashSet<InvoiceService>();
        }
    
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string Notes { get; set; }
        public string ChassisNo { get; set; }
        public string PlateNumber { get; set; }
        public Nullable<int> BrandId { get; set; }
        public int ModelId { get; set; }
        public int km { get; set; }
        public decimal Total { get; set; }
        public decimal NetAmount { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal InvoiceTax { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal NetAmountWithOutTax { get; set; }
        public int StoreID { get; set; }
    
        public virtual CarBrand CarBrand { get; set; }
        public virtual CarModel CarModel { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual Store Store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceService> InvoiceServices { get; set; }
    }
}
