namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Cars = new HashSet<Car>();
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int MemberID { get; set; }

        [Required(ErrorMessage = "Member's last name is required.")]
        [StringLength(30, ErrorMessage = "Member's last name is limited to 30 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Member's first name is required.")]
        [StringLength(30, ErrorMessage = "Member's first name is limited to 30 characters.")]
        public string FirstName { get; set; }

        [StringLength(10, ErrorMessage = "Member's phone number is limited to 10 characters.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Member's address is required.")]
        [StringLength(30, ErrorMessage = "Member's address is limited to 30 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Member's city is required.")]
        [StringLength(30, ErrorMessage = "Member's city is limited to 30 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Member's postal code is required.")]
        [StringLength(6, ErrorMessage = "Member's postal code is limited to 6 characters.")]
        public string PostalCode { get; set; }

        [StringLength(30, ErrorMessage = "Member's email address is limited to 30 characters.")]
        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Member's certification level is required.")]
        [StringLength(1, ErrorMessage = "Member's certification level is limited to a single character.")]
        public string CertificationLevel { get; set; }

        [StringLength(1, ErrorMessage = "Member's gender is limited to a single character.")]
        public string Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }

        public virtual Certification Certification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
