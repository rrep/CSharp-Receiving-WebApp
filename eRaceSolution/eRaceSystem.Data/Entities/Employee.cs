namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Invoices = new HashSet<Invoice>();
            Orders = new HashSet<Order>();
            ReceiveOrders = new HashSet<ReceiveOrder>();
            SalesCartItems = new HashSet<SalesCartItem>();
        }

        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Employee's last name is required.")]
        [StringLength(30, ErrorMessage = "Employee's last name is limited to 30 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Employee's first name is required.")]
        [StringLength(30, ErrorMessage = "Employee's first name is limited to 30 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Employee's address is required.")]
        [StringLength(30, ErrorMessage = "Employee's address is limited to 30 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Employee's city is required.")]
        [StringLength(30, ErrorMessage = "Employee's city is limited to 30 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Employee's postal code is required.")]
        [StringLength(6, ErrorMessage = "Employee's postal code is limited to 6 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Employee's phone number is required.")]
        [StringLength(10, ErrorMessage = "Employee's phone number is limited to 10 characters.")]
        public string Phone { get; set; }

        public int PositionID { get; set; }

        [StringLength(50, ErrorMessage = "Employee's login id is limited to 50 characters.")]
        public string LoginId { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Employee's social insurance number is required.")]
        [StringLength(9, ErrorMessage = "Employee's social insurance number is limited to 9 characters.")]
        public string SocialInsuranceNumber { get; set; }

        public virtual Position Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiveOrder> ReceiveOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesCartItem> SalesCartItems { get; set; }


        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
