using Microsoft.EntityFrameworkCore;
using ReservationApi.Data.Models;

namespace ReservationApi.Data.EFCore.Contexts
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

        public DbSet<Reservation> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
