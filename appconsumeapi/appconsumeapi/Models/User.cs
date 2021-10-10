namespace appconsumeapi.Models
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        [StringLength(50)]
        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[\w-.]+@([\w-]+.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password not match")]
        [NotMapped]
        public string confirm_Password { get; set; }

        public string UImage { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("(01)[0-9]{9}", ErrorMessage = "invalid phone")]
        public string Phone { get; set; }

        public int? BID { get; set; }

        public virtual Branch Branch { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
