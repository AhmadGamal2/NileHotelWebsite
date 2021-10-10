namespace hotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Rooms = new HashSet<Room>();
        }

        [Key]
        public int UID { get; set; }

        [Required]
        [StringLength(30)]
        public string UName { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string Governorate { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public string UImage { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        public int? BID { get; set; }

        public virtual Branch Branch { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
