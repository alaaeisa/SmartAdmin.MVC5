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
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.BalanceItems = new HashSet<BalanceItem>();
            this.InvoiceItems = new HashSet<InvoiceItem>();
            this.StoresBalances = new HashSet<StoresBalance>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public string CarModelsIDs { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public decimal PurchasingPrice { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BalanceItem> BalanceItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoresBalance> StoresBalances { get; set; }
    }
}
