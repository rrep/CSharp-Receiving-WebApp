namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
            OrderDetails = new HashSet<OrderDetail>();
            SalesCartItems = new HashSet<SalesCartItem>();
            StoreRefunds = new HashSet<StoreRefund>();
            VendorCatalogs = new HashSet<VendorCatalog>();
        }

        public int ProductID { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(40, ErrorMessage = "Item name is limited to 40 characters.")]
        public string ItemName { get; set; }

        [Column(TypeName = "money")]
        public decimal ItemPrice { get; set; }

        public int QuantityOnOrder { get; set; }

        public int QuantityOnHand { get; set; }

        public int ReOrderLevel { get; set; }

        [Column(TypeName = "money")]
        public decimal ReStockCharge { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesCartItem> SalesCartItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreRefund> StoreRefunds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorCatalog> VendorCatalogs { get; set; }
    }
}
