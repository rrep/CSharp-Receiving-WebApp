namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int CarID { get; set; }

        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(15, ErrorMessage = "Car description is limited to 15 characters.")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Car ownership is required.")]
        [StringLength(6, ErrorMessage = "Car ownership is limited to 6 characters.")]
        public string Ownership { get; set; }

        public int CarClassID { get; set; }

        [Required(ErrorMessage = "Car state is required.")]
        [StringLength(10, ErrorMessage = "Car state is limited to 10 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Car description is required.")]
        [StringLength(255, ErrorMessage = "Car description is limited to 255 characters.")]
        public string Description { get; set; }

        public int? MemberID { get; set; }

        public virtual CarClass CarClass { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
