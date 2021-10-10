namespace hotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RID { get; set; }

        [Required]
        [StringLength(15)]
        public string type { get; set; }

        public string photo { get; set; }

        [Column(TypeName = "money")]
        public decimal price { get; set; }

        public int No_Beds { get; set; }

        public int? UID { get; set; }

        [Required]
        [StringLength(15)]
        public string Status { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual User User { get; set; }
    }
}
