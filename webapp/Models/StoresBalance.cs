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
    
    public partial class StoresBalance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StoresBalance()
        {
            this.InvoiceServices = new HashSet<InvoiceService>();
        }
    
        public int ID { get; set; }
        public decimal Quantity { get; set; }
        public int ItemID { get; set; }
        public int StoreID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceService> InvoiceServices { get; set; }
        public virtual Item Item { get; set; }
        public virtual Store Store { get; set; }
    }
}
