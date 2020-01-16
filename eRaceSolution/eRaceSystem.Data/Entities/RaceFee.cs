namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RaceFee
    {
        public int RaceFeeID { get; set; }

        [Required(ErrorMessage = "Race fee description is required.")]
        [StringLength(50, ErrorMessage = "Race fee description is limited to 150 characters.")]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Fee { get; set; }
    }
}
