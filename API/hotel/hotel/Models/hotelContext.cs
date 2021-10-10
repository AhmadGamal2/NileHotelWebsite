using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace hotel.Models
{
    public partial class hotelContext : DbContext
    {
        public hotelContext()
            : base("name=hotelContext")
        {
        }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.Branch)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.price)
                .HasPrecision(19, 4);
        }
    }
}
