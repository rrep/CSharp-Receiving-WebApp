namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UnOrderedItem
    {
        [Key]
        public int ItemID { get; set; }
        [NotMapped]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Unordered item name is required.")]
        [StringLength(50, ErrorMessage = "Unordered item name is limited to 50 characters.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Unordered item vendor product ID is required.")]
        [StringLength(25, ErrorMessage = "Unordered item vendor product ID is limited to 25 characters.")]
        public string VendorProductID { get; set; }

        public int Quantity { get; set; }
    }
}
