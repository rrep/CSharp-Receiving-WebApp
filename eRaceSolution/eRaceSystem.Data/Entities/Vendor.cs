namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            Orders = new HashSet<Order>();
            VendorCatalogs = new HashSet<VendorCatalog>();
        }

        public int VendorID { get; set; }

        [Required(ErrorMessage = "Vendor name is required.")]
        [StringLength(30, ErrorMessage = "Vendor name is limited to 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vendor address is required.")]
        [StringLength(30, ErrorMessage = "Vendor address is limited to 30 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vendor city is required.")]
        [StringLength(30, ErrorMessage = "Vendor city is limited to 30 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Vendor postal code is required.")]
        [StringLength(6, ErrorMessage = "Vendor postal code is limited to 6 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Vendor phone number is required.")]
        [StringLength(10, ErrorMessage = "Vendor phone number is limited to 10 characters.")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "Vendor contact is limited to 50 characters.")]
        public string Contact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorCatalog> VendorCatalogs { get; set; }
    }
}
